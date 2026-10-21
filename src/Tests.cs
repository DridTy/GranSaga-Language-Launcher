using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace GranSagaLanguages {
 static class Tests {
  static void Assert(bool ok,string message){if(!ok)throw new Exception("TEST FAILED: "+message);}
  static void MustFail(Action action){bool failed=false;try{action();}catch{failed=true;}Assert(failed,"expected rejection");}
  public static void Run(Engine e,string fixture) {
   fixture=Path.GetFullPath(fixture);string original=Path.Combine(fixture,"original.pak");
   Assert(Engine.Hash(original)==e.Data.originalSha256,"original fixture hash");
   string game=Path.Combine(fixture,"fake-game");string target=Path.Combine(game,Engine.RelativePak);
   Directory.CreateDirectory(Path.GetDirectoryName(target));File.Copy(original,target,true);
   string sig=Path.ChangeExtension(target,".sig");File.WriteAllText(sig,"TEST SIGNATURE",Encoding.ASCII);string sigHash=Engine.Hash(sig);
   var passed=new List<string>();
   foreach(var lang in e.Data.languages) {
    string rebuilt=Path.Combine(fixture,"reconstruct-"+lang.id+".pak");if(File.Exists(rebuilt))File.Delete(rebuilt);
    e.Reconstruct(original,lang,rebuilt);Assert(Engine.Hash(rebuilt)==lang.targetSha256,"reconstruct "+lang.id);
    passed.Add("Patch reconstructs exact "+lang.id+" SHA256");
   }
   foreach(var lang in e.Data.languages) {
    e.Apply(game,lang,true);Assert(Engine.Hash(target)==lang.targetSha256,"install "+lang.id);
    e.Apply(game,lang,true);Assert(Engine.Hash(target)==lang.targetSha256,"idempotent "+lang.id);
    Assert(Engine.Hash(sig)==sigHash,"signature unchanged");passed.Add("Install, verify, repeat "+lang.id);
   }
   string store=Path.Combine(Path.GetDirectoryName(target),"GranSagaLanguageBackup");
   Assert(Engine.Hash(Path.Combine(store,"original.pak"))==e.Data.originalSha256,"verified original backup");
   passed.Add("Original backup and untouched signature verified");
   string oldHash=Engine.Hash(target);using(var f=File.Open(target,FileMode.Open,FileAccess.Write)){f.Position=100;f.WriteByte(123);}
   string bad=Engine.Hash(target);MustFail(()=>e.Apply(game,e.Data.languages[0],true));Assert(Engine.Hash(target)==bad,"unknown version unchanged");
   File.Copy(original,target,true);passed.Add("Unknown game build rejected without mutation");
   using(var handle=new FileStream(Path.Combine(store,"install.lock"),FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None)) {
    MustFail(()=>e.Apply(game,e.Data.languages[0],true));Assert(Engine.Hash(target)==oldHash,"locked install unchanged");
   }
   passed.Add("Concurrent installer lock enforced");
   string backup=Path.Combine(store,"original.pak");File.WriteAllText(backup,"BAD BACKUP");
   MustFail(()=>e.Apply(game,e.Data.languages[0],true));Assert(Engine.Hash(target)==oldHash,"bad backup does not alter target");
   File.Copy(original,backup,true);passed.Add("Corrupt backup rejected without mutation");
   string corruptRoot=Path.Combine(fixture,"corrupt-distribution");Directory.CreateDirectory(Path.Combine(corruptRoot,"patches"));
   File.Copy(Path.Combine(e.Root,"manifest.json"),Path.Combine(corruptRoot,"manifest.json"),true);
   File.WriteAllText(Path.Combine(corruptRoot,"patches",e.Data.languages[0].file),"BROKEN PATCH");
   var corrupt=new Engine(corruptRoot);MustFail(()=>corrupt.Apply(game,e.Data.languages[0],true));Assert(Engine.Hash(target)==oldHash,"corrupt patch target unchanged");
   passed.Add("Corrupt patch rejected before replacing game data");
   // First installation over an already modified v0.4: reconstruct a clean backup.
   File.Copy(Path.Combine(fixture,"reconstruct-pt-BR.pak"),target,true);File.Delete(backup);
   e.Apply(game,e.Data.languages.Single(x=>x.id=="en-US"),true);
   Assert(Engine.Hash(backup)==e.Data.originalSha256,"recover original from existing mod");
   e.Apply(game,e.Data.languages.Single(x=>x.id=="ru-RU"),true);
   Assert(Engine.Hash(target)==e.Data.originalSha256,"restore after switch");
   Assert(Engine.Hash(sig)==sigHash,"final signature hash");passed.Add("Recover clean original from v0.4, switch to EN and restore RU");
   string previousEnglish=Path.Combine(fixture,"previous-English.pak");
   if(File.Exists(previousEnglish)) {
    File.Copy(previousEnglish,target,true);
    e.Apply(game,e.Data.languages.Single(x=>x.id=="en-US"),true);
    Assert(Engine.Hash(target)==e.Data.languages.Single(x=>x.id=="en-US").targetSha256,"upgrade previous English");
    Assert(Engine.Hash(backup)==e.Data.originalSha256,"upgrade preserves original backup");
    Assert(Engine.Hash(sig)==sigHash,"upgrade preserves signature");
    passed.Add("Upgrade directly from previous English without restoring RU first");
   }
   File.WriteAllLines(Path.Combine(e.Root,"TEST-RESULTS.txt"),passed.Concat(new[]{"PASS: "+passed.Count+" integration checks. Fake installation only; no live game altered."}),Encoding.UTF8);
  }
 }
}

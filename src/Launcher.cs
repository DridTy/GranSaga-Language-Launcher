using System;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GranSagaLanguages {
 public class Patch {
  public string id, label, file, sha256, targetSha256, description;
  public long prefixLength, targetLength;
  public override string ToString() { return label; }
 }
 public class Manifest {
  public string version, originalSha256, prefixSha256;
  public string[] supportedHashes;
  public Patch[] languages;
 }
 public class Engine {
  public readonly string Root;
  public readonly Manifest Data;
  public const string RelativePak = @"ProjectR\PersistentDownloadDir\ndb\ProjectR-WindowsNoEditor.pak";
  public Engine(string root) {
   Root=Path.GetFullPath(root);
   Data=new JavaScriptSerializer().Deserialize<Manifest>(File.ReadAllText(Path.Combine(Root,"manifest.json"),Encoding.UTF8));
   if(Data==null || Data.languages==null || Data.languages.Length!=3 || Data.supportedHashes==null) throw new InvalidDataException("Manifesto inválido / Invalid manifest.");
   foreach(var p in Data.languages) {
    if(p.file!=Path.GetFileName(p.file) || p.targetLength<1 || p.targetLength>268435456 || p.prefixLength<0 || p.prefixLength>p.targetLength) throw new InvalidDataException("Pacote inválido / Invalid package.");
   }
  }
  public static string Hash(string path) { using(var f=File.OpenRead(path)) using(var h=SHA256.Create()) return Hex(h.ComputeHash(f)); }
  static string Hex(byte[] b) { return BitConverter.ToString(b).Replace("-","").ToLowerInvariant(); }
  static string PrefixHash(string path,long length) {
   using(var f=File.OpenRead(path)) using(var h=SHA256.Create()) {
    byte[] b=new byte[1048576]; long left=length;
    while(left>0) { int n=f.Read(b,0,(int)Math.Min(b.Length,left)); if(n==0)throw new EndOfStreamException(); h.TransformBlock(b,0,n,b,0);left-=n; }
    h.TransformFinalBlock(new byte[0],0,0);return Hex(h.Hash);
   }
  }
  public static bool GameRunning() {
   foreach(var p in Process.GetProcesses()) using(p) { try { string n=p.ProcessName; if(n.Equals("GranSaga",StringComparison.OrdinalIgnoreCase) || n.StartsWith("ProjectR",StringComparison.OrdinalIgnoreCase))return true; } catch(InvalidOperationException){} }
   return false;
  }
  public string Inspect(string folder) {
   string target=Path.Combine(Path.GetFullPath(folder),RelativePak);
   if(!File.Exists(target))throw new FileNotFoundException("Selecione a pasta GranSaga que contém ProjectR. / Select the GranSaga folder containing ProjectR.");
   string hash=Hash(target);
   if(!Data.supportedHashes.Contains(hash)) throw new InvalidDataException("Versão desconhecida. Nenhum arquivo foi alterado. Use uma versão compatível ou aguarde uma atualização do mod. / Unsupported game version. No files changed.");
   return hash;
  }
  public void Reconstruct(string source,Patch patch,string destination) {
   string patchPath=Path.Combine(Root,"patches",patch.file);
   if(Hash(patchPath)!=patch.sha256)throw new InvalidDataException("Patch corrompido / Corrupt patch.");
   if(PrefixHash(source,patch.prefixLength)!=Data.prefixSha256)throw new InvalidDataException("Base incompatível / Incompatible base.");
   using(var dst=new FileStream(destination,FileMode.CreateNew,FileAccess.Write,FileShare.None)) {
    using(var src=File.OpenRead(source)) {
     byte[] b=new byte[1048576];long left=patch.prefixLength;
     while(left>0) { int n=src.Read(b,0,(int)Math.Min(b.Length,left));if(n==0)throw new EndOfStreamException();dst.Write(b,0,n);left-=n; }
    }
    using(var file=File.OpenRead(patchPath)) using(var gz=new GZipStream(file,CompressionMode.Decompress)) {
     byte[] b=new byte[1048576];long left=patch.targetLength-patch.prefixLength;
     while(left>0) { int n=gz.Read(b,0,(int)Math.Min(b.Length,left));if(n==0)throw new EndOfStreamException();dst.Write(b,0,n);left-=n; }
     if(gz.ReadByte()!=-1)throw new InvalidDataException("Patch maior que o esperado / Oversized patch.");
    }
    dst.Flush(true);
   }
   if(Hash(destination)!=patch.targetSha256)throw new InvalidDataException("Falha na verificação do resultado / Output verification failed.");
  }
  public string Apply(string folder,Patch selected,bool testMode=false) {
   if(!testMode && GameRunning())throw new IOException("Feche o jogo antes de instalar. / Close the game before installing.");
   folder=Path.GetFullPath(folder);string target=Path.Combine(folder,RelativePak);
   string before=Inspect(folder);
   string store=Path.Combine(Path.GetDirectoryName(target),"GranSagaLanguageBackup");
   Directory.CreateDirectory(store);
   string mutexPath=Path.Combine(store,"install.lock");
   using(var installLock=new FileStream(mutexPath,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None)) {
    if(Inspect(folder)!=before)throw new IOException("O pacote mudou. Tente novamente. / Package changed. Try again.");
    if(before==selected.targetSha256)return "Este idioma já está aplicado. / This language is already installed.";
    string signature=Path.ChangeExtension(target,".sig");
    if(!File.Exists(signature))throw new FileNotFoundException("Assinatura original ausente / Original signature missing.");
    string signatureHash=Hash(signature);
    string original=Path.Combine(store,"original.pak");
    Patch russian=Data.languages.Single(x=>x.id=="ru-RU");
    string temp=Path.Combine(Path.GetDirectoryName(target),".gransaga-"+Guid.NewGuid().ToString("N")+".tmp");
    string backupTemp=Path.Combine(store,Guid.NewGuid().ToString("N")+".tmp");
    try {
     if(File.Exists(original)) {
      if(Hash(original)!=Data.originalSha256)throw new InvalidDataException("Backup original inválido; ele não será sobrescrito. / Original backup is invalid and will not be overwritten.");
     } else {
      Reconstruct(target,russian,backupTemp);
      File.Move(backupTemp,original);
     }
     Reconstruct(original,selected,temp);
     if(Hash(target)!=before || Hash(signature)!=signatureHash)throw new IOException("Arquivos mudaram durante a operação. / Files changed during installation.");
     if(!testMode && GameRunning())throw new IOException("O jogo foi aberto. Feche-o e tente novamente. / Game was opened. Close it and retry.");
     string rollback=Path.Combine(store,"previous-"+DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")+"-"+Guid.NewGuid().ToString("N")+".pak");
     // Atomic replacement on the same volume. Keep the previous version for recovery.
     File.Replace(temp,target,rollback,true);
     try {
      if(Hash(target)!=selected.targetSha256 || Hash(signature)!=signatureHash)throw new IOException("Verificação final falhou. / Final verification failed.");
     } catch { File.Replace(rollback,target,null,true);throw; }
     // Metadata is best effort: a settings write must never undo a verified install.
     try { File.WriteAllText(Path.Combine(store,"last-install.txt"),selected.id+Environment.NewLine+selected.targetSha256,Encoding.UTF8); } catch(IOException){} catch(UnauthorizedAccessException){}
     return "Idioma aplicado. Abra o jogo pelo 4game. / Language installed. Start the game through 4game.";
    } finally { if(File.Exists(temp))File.Delete(temp);if(File.Exists(backupTemp))File.Delete(backupTemp); }
   }
  }
 }
 public class Launcher : Form {
  readonly Engine engine;
  TextBox folder; FlowLayoutPanel languages; Patch selectedLanguage; Label details,status; ProgressBar progress;
  Button install,restore,browse,check,open;
  Color bg=Color.FromArgb(19,25,39), card=Color.FromArgb(30,40,60), gold=Color.FromArgb(230,194,117);
  public Launcher(Engine e) {
   engine=e; Text="Gran Saga · Language Launcher";ClientSize=new Size(850,630);MinimumSize=new Size(866,669);
   BackColor=bg;ForeColor=Color.White;Font=new Font("Segoe UI",10);StartPosition=FormStartPosition.CenterScreen;AutoScaleMode=AutoScaleMode.Dpi;
   var main=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(30),ColumnCount=1,RowCount=9};Controls.Add(main);
   int[] heights={46,40,26,45,26,48,100,65,55};foreach(int h in heights)main.RowStyles.Add(new RowStyle(SizeType.Absolute,h));
   main.Controls.Add(new Label{Text="GRAN SAGA  /  LANGUAGES",ForeColor=gold,Font=new Font("Segoe UI Semibold",22),AutoSize=true},0,0);
   main.Controls.Add(new Label{Text="Instale uma tradução · Install a translation",AutoSize=true,ForeColor=Color.LightSteelBlue},0,1);
   main.Controls.Add(new Label{Text="Pasta do jogo / Game folder",AutoSize=true},0,2);
   var pathRow=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=2};pathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));pathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,145));
   folder=new TextBox{Dock=DockStyle.Fill,Text=@"C:\Games\GranSaga",BackColor=card,ForeColor=Color.White,BorderStyle=BorderStyle.FixedSingle};
   browse=Button("Procurar / Browse");browse.Click+=(s,a)=>{using(var d=new FolderBrowserDialog()){d.Description="Selecione a pasta GranSaga / Select the GranSaga folder";if(Directory.Exists(folder.Text))d.SelectedPath=folder.Text;if(d.ShowDialog()==DialogResult.OK)folder.Text=d.SelectedPath;}};
   pathRow.Controls.Add(folder,0,0);pathRow.Controls.Add(browse,1,0);main.Controls.Add(pathRow,0,3);
   main.Controls.Add(new Label{Text="Idioma do mod / Mod language",AutoSize=true},0,4);
   languages=new FlowLayoutPanel{Dock=DockStyle.Fill,BackColor=card,Padding=new Padding(6),WrapContents=false};main.Controls.Add(languages,0,5);
   details=new Label{Dock=DockStyle.Fill,Padding=new Padding(12),BackColor=card,ForeColor=Color.LightSteelBlue};main.Controls.Add(details,0,6);
   foreach(var p in e.Data.languages) {
    Patch choice=p;
    string label=p.id=="pt-BR"?"Português BR · v0.4":p.id=="en-US"?"English · progression v0.2":"Русский · original";
    var radio=new RadioButton{Text=label,Width=245,Height=28,ForeColor=Color.White,BackColor=card,Tag=choice};
    radio.CheckedChanged+=(s,a)=>{if(radio.Checked){selectedLanguage=choice;details.Text=choice.description;}};
    languages.Controls.Add(radio);if(selectedLanguage==null)radio.Checked=true;
   }
   var actionRow=new FlowLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(0,10,0,0),WrapContents=false};
   install=Button("Aplicar / Apply");install.BackColor=gold;install.ForeColor=bg;install.Click+=async(s,a)=>await Action(false);
   restore=Button("Restaurar russo / Restore RU");restore.Width=220;restore.Click+=async(s,a)=>await Action(true);
   check=Button("Verificar / Check");check.Click+=async(s,a)=>await Inspect();
   open=Button("Abrir 4game...");open.Click+=(s,a)=>Open4game();
   actionRow.Controls.AddRange(new Control[]{install,restore,check,open});main.Controls.Add(actionRow,0,7);
   var bottom=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=1,RowCount=2};bottom.RowStyles.Add(new RowStyle(SizeType.Absolute,38));bottom.RowStyles.Add(new RowStyle(SizeType.Absolute,8));
   status=new Label{Dock=DockStyle.Fill,Text="Feche o jogo antes de aplicar. / Close the game before applying.",ForeColor=Color.LightSteelBlue,AutoEllipsis=true};
   progress=new ProgressBar{Dock=DockStyle.Fill,Style=ProgressBarStyle.Marquee,Visible=false};bottom.Controls.Add(status,0,0);bottom.Controls.Add(progress,0,1);main.Controls.Add(bottom,0,8);
  }
  Button Button(string text) {return new Button{Text=text,Width=145,Height=36,FlatStyle=FlatStyle.Flat,BackColor=card,ForeColor=Color.White,Cursor=Cursors.Hand,Margin=new Padding(0,0,8,0)};}
  bool busy;
  void Busy(bool value) {busy=value;foreach(Control c in new Control[]{folder,languages,install,restore,browse,check,open})c.Enabled=!value;progress.Visible=value;UseWaitCursor=value;}
  protected override void OnFormClosing(FormClosingEventArgs e){if(busy){e.Cancel=true;return;}base.OnFormClosing(e);}
  async Task Action(bool reset) {
   string path=folder.Text;Patch choice=reset?engine.Data.languages.Single(x=>x.id=="ru-RU"):selectedLanguage;
   Busy(true);status.Text="Verificando e preparando backup... / Verifying and preparing backup...";
   try {string result=await Task.Run(()=>engine.Apply(path,choice));status.Text=result;MessageBox.Show(this,result,"Gran Saga Languages",MessageBoxButtons.OK,MessageBoxIcon.Information);}
   catch(UnauthorizedAccessException){Error("Sem permissão de escrita. Feche o app e execute como administrador se a pasta exigir. / Write access denied. Run as administrator if needed.");}
   catch(Exception ex){Error(ex.Message);}finally{Busy(false);}
  }
  async Task Inspect() {
   string path=folder.Text;Busy(true);status.Text="Verificando... / Checking...";
   try {string hash=await Task.Run(()=>engine.Inspect(path));Patch p=engine.Data.languages.FirstOrDefault(x=>x.targetSha256==hash);status.Text="Compatível / Compatible: "+(p==null?"PT-BR anterior / previous PT-BR":p.label);}
   catch(Exception ex){Error(ex.Message);}finally{Busy(false);}
  }
  void Error(string message){status.Text="Operação interrompida / Operation stopped";MessageBox.Show(this,message,"Gran Saga Languages",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
  void Open4game() {
   using(var d=new OpenFileDialog{Title="Selecione o aplicativo ou atalho do 4game / Select 4game",Filter="4game (*.exe;*.lnk)|*.exe;*.lnk",CheckFileExists=true}) {
    if(d.ShowDialog()!=DialogResult.OK)return;
    if(!Path.GetFileNameWithoutExtension(d.FileName).ToLowerInvariant().Contains("4game")){Error("Selecione o 4game. / Select 4game.");return;}
    try{Process.Start(new ProcessStartInfo(d.FileName){UseShellExecute=true});}catch(Exception ex){Error(ex.Message);}
   }
  }
 }
 static class Program {
  [STAThread] static int Main(string[] args) {
   try {
    string root=AppDomain.CurrentDomain.BaseDirectory;var engine=new Engine(root);
    if(args.Length>0 && args[0]=="--self-test") { Tests.Run(engine,args[1]);return 0; }
    Application.EnableVisualStyles();Application.SetCompatibleTextRenderingDefault(false);
    using(var form=new Launcher(engine)) {
     if(args.Length==2 && args[0]=="--preview") {form.CreateControl();form.Show();Application.DoEvents();using(var b=new Bitmap(form.Width,form.Height)){form.DrawToBitmap(b,new Rectangle(0,0,form.Width,form.Height));b.Save(args[1]);}return 0;}
     Application.Run(form);
    }return 0;
   } catch(Exception e) {if(args.Length>0){Console.Error.WriteLine(e);return 1;}MessageBox.Show(e.Message,"Gran Saga Languages",MessageBoxButtons.OK,MessageBoxIcon.Error);return 1;}
  }
 }
}

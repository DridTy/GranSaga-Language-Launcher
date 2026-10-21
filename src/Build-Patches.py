"""Package already validated PAKs; no extraction keys or game executable needed."""
import argparse
import gzip
import hashlib
import json
import struct
from pathlib import Path

def digest(data):
    return hashlib.sha256(data).hexdigest()

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--original', type=Path, required=True)
    parser.add_argument('--pt', type=Path, required=True)
    parser.add_argument('--en', type=Path, required=True)
    parser.add_argument('--distribution', type=Path, default=Path(__file__).resolve().parent.parent)
    args = parser.parse_args()
    manifest_path = args.distribution / 'manifest.json'
    manifest = json.loads(manifest_path.read_text(encoding='utf-8'))
    original = args.original.read_bytes()
    if digest(original) != manifest['originalSha256']:
        raise ValueError('Original build is incompatible; regenerate and validate the mod first.')
    footer = original[-221:]
    magic, version, prefix = struct.unpack_from('<IIQ', footer, 17)
    if (magic, version) != (0x5a6f12e1, 8):
        raise ValueError('Unsupported PAK footer')
    files = {'pt-BR': args.pt, 'en-US': args.en, 'ru-RU': args.original}
    prepared = []
    for patch in manifest['languages']:
        data = files[patch['id']].read_bytes()
        if data[:prefix] != original[:prefix]:
            raise ValueError('Modified PAK does not have the unchanged original prefix')
        if digest(data) != patch['targetSha256']:
            raise ValueError('Target differs from this validated release; review new version metadata first')
        compressed = gzip.compress(data[prefix:], compresslevel=9, mtime=0)
        if digest(compressed) != patch['sha256']:
            raise ValueError('Patch differs from the release manifest')
        prepared.append((args.distribution / 'patches' / patch['file'], compressed))
    for path, compressed in prepared:
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(compressed)
    print('Reproduced all three validated patches.')

if __name__ == '__main__':
    main()

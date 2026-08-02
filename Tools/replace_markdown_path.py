"""Replace one exact path prefix in Markdown files without changing other bytes."""

from __future__ import annotations

import argparse
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("directory", type=Path)
    parser.add_argument("old", help="Exact byte-compatible path prefix to replace")
    parser.add_argument("new", help="Replacement path prefix")
    args = parser.parse_args()

    old = args.old.encode("utf-8")
    new = args.new.encode("utf-8")
    files_changed = 0
    replacements = 0

    for path in sorted(args.directory.glob("*.md")):
        original = path.read_bytes()
        count = original.count(old)
        if not count:
            continue
        path.write_bytes(original.replace(old, new))
        files_changed += 1
        replacements += count
        print(f"updated {path}: {count} replacement(s)")

    print(f"files_changed={files_changed} replacements={replacements}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

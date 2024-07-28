rmdir /s /q dist build
python -m PyInstaller --one-file ./installer.py

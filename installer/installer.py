import tempfile
import urllib.request
import os
import zipfile
import ctypes
import shutil
import sys
import subprocess

def error(msg):
    ctypes.windll.user32.MessageBoxW(0, msg, "OzStrips Error", 1)

try:
    url = "https://github.com/maxrumsey/OzStrips/releases/download/latest/Plugin.zip"

    print(sys.argv)

    if len(sys.argv) > 1:
        url = sys.argv[1]
    
    os.system("taskkill /im vatSys.exe")
    tempdir = tempfile.gettempdir() + "\\ozstrips\\"

    plugindir = os.path.expanduser("~\\Documents\\vatSys Files\\Profiles\\Australia\\Plugins")
    if os.path.exists(tempdir):
        shutil.rmtree(tempdir)

    os.makedirs(tempdir)
    os.makedirs(tempdir + "\\OzStrips")

    urllib.request.urlretrieve(url, tempdir + "plugin.zip")

    if not os.path.exists(tempdir + "plugin.zip"):
        error("Failed to download plugin.zip")
        sys.exit(1)

    with zipfile.ZipFile(tempdir + "plugin.zip", 'r') as zip_ref:
        zip_ref.extractall(tempdir + "\\OzStrips")

    if sys.argv[0].endswith(".exe"):
        shutil.move(sys.argv[0], tempdir + "\\installer.backup")

    try:
        if os.path.exists(plugindir + "\\OzStrips"):
            shutil.rmtree(plugindir + "\\OzStrips")
    except:
        print("Not all items could be removed.")
        
    shutil.copytree(tempdir + "OzStrips", plugindir + "\\OzStrips", dirs_exist_ok=True)

    proc_exe = subprocess.call("C:\\Program Files (x86)\\vatSys\\bin\\vatSys.exe", cwd="C:\\Program Files (x86)\\vatSys\\bin\\")

except:
    error("A fatal error occurred while updating. Please reinstall OzStrips!")
    sys.exit(1)
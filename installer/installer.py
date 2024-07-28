import tempfile
import urllib.request
import os
import zipfile
import ctypes
import shutil
import sys
import subprocess
import time

def error(msg):
    ctypes.windll.user32.MessageBoxW(0, msg, "OzStrips Error", 1)

try:
    url = "https://github.com/maxrumsey/OzStrips/releases/download/latest/Plugin.zip"
    plugindir = os.path.expanduser("~\\Documents\\vatSys Files\\Profiles\\Australia\\Plugins")
    
    print(sys.argv)

    if len(sys.argv) > 2:
        url = sys.argv[1]
        plugindir = sys.argv[2]
    
    os.system("taskkill /im vatSys.exe")
    time.sleep(4)
    tempdir = tempfile.gettempdir() + "\\ozstrips\\"

    try:
        if os.path.exists(tempdir + "\\OzStrips\\"):
            shutil.rmtree(tempdir + "\\OzStrips\\")

        if os.path.exists(tempdir + "plugin.zip"):
            os.remove(tempdir + "plugin.zip")

        if (not os.path.exists(tempdir)): os.makedirs(tempdir)
        if (not os.path.exists(tempdir + "\\OzStrips")): os.makedirs(tempdir + "\\OzStrips")
    except shutil.Error as e:
        print(e)

    urllib.request.urlretrieve(url, tempdir + "plugin.zip")

    if not os.path.exists(tempdir + "plugin.zip"):
        error("Failed to download plugin.zip")
        sys.exit(1)

    with zipfile.ZipFile(tempdir + "plugin.zip", 'r') as zip_ref:
        zip_ref.extractall(tempdir + "\\OzStrips")

    try:
        if os.path.exists(plugindir + "\\OzStrips"):
            shutil.rmtree(plugindir + "\\OzStrips")
    except:
        print("Not all items could be removed.")
        
    shutil.copytree(tempdir + "OzStrips", plugindir + "\\OzStrips", dirs_exist_ok=True)

    proc_exe = subprocess.call("C:\\Program Files (x86)\\vatSys\\bin\\vatSys.exe", cwd="C:\\Program Files (x86)\\vatSys\\bin\\")

except Exception as e:
    print(str(e))
    error("A fatal error occurred while updating. Please reinstall OzStrips!\n")
    sys.exit(1)
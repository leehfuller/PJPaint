// get required objects
var shell = WScript.Createobject("Wscript.shell");
var fso = WScript.Createobject("Scripting.FileSystemObject");
var desktop = shell.SpecialFolders("Desktop");

// create or update new shortcut
var deskIcon = shell.CreateShortcut(desktop + "\\PJPaint.lnk");
deskIcon.TargetPath = "c:\\program files\\PJPaint\PJPaint.exe";
deskIcon.IconLocation = "c:\\program files\\PJPaint\PJPaint.ico";
deskIcon.Save();

// free objects
shell = null
desktop = null;

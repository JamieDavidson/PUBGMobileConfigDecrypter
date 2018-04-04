## Warning: Because of the Xor operator used in combination with a static key, some (if not all) antiviruses will flag this as malware. This poor encryption technique is commonly associated with malware, most commonly in poorly written ransomware. Blame PUBG Mobile publishers for encrypting their files this way.

## Make sure you place your UserCustom.ini file in the same folder as the executable.
### Help text:
```
Usage: crypter [OPTIONS]
Encrypts or decrypts a PUBG Mobile UserCustom.ini file.
May not work for future versions if encryption method changes
Options:
  -d, --decrypt              Specifies that the file should be decrypted
  -e, --encrypt              Specifies that the file should be encrypted
  -f, --file                 File path of the file to be encrypted/decrypted
  -h, --help                 Show this message and exit
```

### To decrypt a file:
Open your favourite command line tool (Command Prompt, Powershell, Cygwin etc).
You can easily do this by holding shift and right clicking on empty space in File Explorer.
Navigate to the folder that the binary is in and type the following:
```
.\crypter.exe -d -f decrypted.txt
```

This will decrypt UserCustom.ini and create a file called `decrypted.txt` with the result.

### To encrypt a file:
```
.\crypter.exe -e -f decrypted.txt
```

This will take the decrypted file and turn it back in to a UserCustom.ini file.

## Warning: This will overwrite files by default.

```
TODO:
Catch IO exceptions when reading/writing files
Replace command line options parsing library with one that's more robust
```

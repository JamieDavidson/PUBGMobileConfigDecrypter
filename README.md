# PUBGMobileConfigDecrypter
This was a quick and easy thing that I threw together, it's not well written and may be flaky.

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
Open command prompt, powershell or similar tools. Navigate to the folder that the binary is in and type the following:
```
crypter.exe -d -f decrypted.txt
```

This will decrypt UserCustom.ini and create a file called `decrypted.txt` with the result.

### To encrypt a file:
```
crypter.exe -e -f decrypted.txt
```

This will take the decrypted file and turn it back in to a UserCustom.ini file.

## Warning: This will overwrite files by default.
# DirectoryCleaner

DirectoryCleaner  is the console application to remove old unnecessary backup files, leaving one file per month.
It searches the directory looking for the files with the specified mask and modification date.
Warning!! The day in which the backup has been created is recognized after the last modification date.

## Arguments

Application can take fillowing arguments:

### Required

* -p - Path to the backup folder
* -m - Backup file name mask. Can contain the wildcards "?" and "*"

### Optional

* -s - Leave the backup from the specified day. 1 by default, if the parameter is not passed.
* -d - Days back condition to search the files. The files that are younger then specified number of days will not be deleted. 30 by default.
* -t - Test parameter. When passed, the program will list the files to be removed, but not remove them.

## Exmaple call

Below is the example of calling the application from the command line

> DirectoryCleaner.exe -p Z:\backup\files -m *backup??????.zip -s 20 -d 60 -t

In the above example, the programm will find all the files in the *Z:\backup\files* directory that match the **_map_??????.zip* mask. All the files will be from the 20th day of the month and will be older than 60 days. The program will output the full path of the files that are subject to deletion.  

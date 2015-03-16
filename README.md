# WinImageSnap
Command line utility to snap a frame from your webcam. Ideal for taking a pretty picture of your face when you git commit!

# Git setup
Add the following line to post-commit in <repositorydirectory>\.git\hooks :
```
<path-to-executable>/WinImageSnap.exe -o <output-folder> -r <repository-name> -w 2 -v -c <pipe-separated-list-of-webcams-in-preferred-order>
```
## Example
I have two webcams, one internal and one attached to the monitor. I set the latter as preferred, and winimagesnap will fall back to the former when needed (e.g when the MS cam isn't connected).
I also have winimagesnap "installed"in (xcopyed to) c:\winimagesnap, and my snaps are placed in subfolders of c:\gitshots.

```
/c/WinImageSnap/WinImageSnap.exe -o /c/gitshots -r WinImageSnap -w 2 -v -c "Microsoft LifeCam Studio|HP HD Webcam [Fixed]"
```

# Command line parameters
```
'c', "cameraNames"	The device (camera) names to use for snapping, in preferred order.
'r', "repositoryName"	The current repository name
'w', "maxWaitTime"	The max time to wait for a snap to be taken.
'o', "outputFolder"	The root output folder. Snaps will be put in repository-named subfolders of this folder.
'v', "verbose"		Verbose output.
'l', "list"		A list of the camera winimagesnap can detect. These names can be used with the 'c' argument.
```

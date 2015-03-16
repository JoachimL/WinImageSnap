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

IsoVoxel
========

Generates isometric pixel art from MagicaVoxel .vox files.

Usage
=====

IsoVoxel is a command-line program, and can be downloaded from this project's [Releases section](https://github.com/tommyettinger/IsoVoxel/releases). From the command line, use `IsoVoxel.exe file.vox x y z o` where file.vox was saved from MagicaVoxel, x, y, and z are the bounds of the file, and o is an outline mode (if you don't include x y z it will use the size of the model as set in MagicaVoxel, if you don't give a .vox file it defaults to Truck.vox , which is included, and if you don't set include o it will use full outlining.  The bounds can all be increased up to a max of approximately 128). The argument o can be one of the following or can be omitted: `outline=full` (the default, black outlines around the edge of the model and shading on inner gaps), `outline=light` (which uses the shaded color instead of black for outer outlines), `outline=partial` (which has no outer outlines but keeps shaded inner outlines), and `outline=none` (which has no inner or outer outlines). The arguments x y z can be omitted, and o can be placed as the last argument even if some or all of x y z have not been included. IsoVoxel will create a subdirectory named after the model (running on Truck.vox will create a folder called Truck) and fill it with 16 images: four for north/south/east/west, four for the diagonals between them, and an additional eight images that are each double-sized versions of one of the origial eight (which may have a significantly better appearance on certain models, particularly for north/south/east/west). It needs .NET 3.5 or higher (as of the time of writing, the current version available on Windows is at least 4.5), and has been confirmed at least once to work on Mono.

Results
=======

![Tank](http://i.imgur.com/4dHLspK.png)
![Tank](http://i.imgur.com/BCe7tFl.png)
![Tank](http://i.imgur.com/P4H7W7Q.png)
![Tank](http://i.imgur.com/Fr6QpcR.png)
![Truck](http://i.imgur.com/eyKMYSu.png)
![Truck](http://i.imgur.com/RVa17b8.png)
![Truck](http://i.imgur.com/HxFCaaz.png)
![Truck](http://i.imgur.com/G6dkG2J.png)


![Tank](http://i.imgur.com/m2bjFBG.png)
![Tank](http://i.imgur.com/InLx1F4.png)
![Tank](http://i.imgur.com/iSlsC39.png)
![Tank](http://i.imgur.com/d8ubLGe.png)
![Truck](http://i.imgur.com/Vqm9K4a.png)
![Truck](http://i.imgur.com/7m3NETe.png)
![Truck](http://i.imgur.com/0f6jUdQ.png)
![Truck](http://i.imgur.com/Z6kjLN9.png)

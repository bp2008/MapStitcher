# MapStitcher
A simple Windows program to bulk-download map tiles and stitch them together.

The default map source is Google Maps' satellite imagery, but MapStitcher should in theory be compatible with any map server which shares a similar API and uses the Mercator projection.

This program has a hard size limit of 10922 tiles per stitching operation, due to the maximum capacity of a byte array.

## Usage

1. Download from the [releases tab](https://github.com/bp2008/MapStitcher/releases) and extract to a location where you have write-permission (such as your desktop).  Run MapStitcher.exe.

2. Enter latitude and longitude of the upper-left and lower-right corners of the region you wish to create a map for.  A preview of the selected region will appear, at a zoom level appropriate for showing the entire region.  The zoom level you see in the preview is not representative of the map tiles which will be downloaded and stitched.

3. Enter a Zoom level between 0 and 23.  Note that high zoom levels are not available in many parts of the world, and 20 is commonly the limit.

4. Click `Download Maps` and wait until the process is complete.

5. Click `Stitch Into One Map` and wait until the process is complete.  Upon successful stitching, the finished map will be saved to the current directory and opened automatically.  If this part fails during the final compression stage, you may need to split your map into two sections.

![Screenshot](https://i.imgur.com/uToQ6vk.jpg)

## Tips

* This program was created in one evening, and has a lot of rough edges.  Please be patient with it.
* If errors occur while downloading maps, just click the button again to retry.  If the errors persist, you may have selected a zoom level higer than the server is able to deliver for your selected region.  All map tiles are saved to a `MapCache` subdirectory, so clicking the download button again will only get any tiles which are missing.
* If you close the program while downloading map tiles, any tiles that were currently being written may be corrupted on disk.  You'll notice them when you view the final stitched output.  If this happens, find the bad tiles in the MapCache directory and delete them.
* Large numbers of tiles approaching the hard limit of 10922 may cause the jpeg compressor to crash.  I currently have no solution for this except to suggest trying a smaller region or lower zoom factor.

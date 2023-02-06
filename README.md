# TiniTools
1. Texture Resize : Resize a texture size that can be divided by 4 and can be compressed by Unity. 
* It works like this: create a texture that is slightly larger than the original, then place the original over the newly created texture with no resize algorithm.

# How To Install
1. TextureResize
* Get the script at assets/TextureResize
* Put the script to your unity project
* let unity compile
* To use the Tools open it at menu bar "Tools"

# How To Use
1.  Texture Resize
* Pick a texture that you want to resize
* Allow read/write
* Select "Tools" from menu bar
* Click sub menu "Resize Texture To Nearest 4x4"

* Click Sub menu "Check Texture 4x4" to Check all texture
* Disable read/write when done.

# Limitation
1. Texture Resize:
  - May contains white/transparent border if selected texture is RGB. consider resize with photo editor, etc.
  - Resized texture or sprite content may shift.

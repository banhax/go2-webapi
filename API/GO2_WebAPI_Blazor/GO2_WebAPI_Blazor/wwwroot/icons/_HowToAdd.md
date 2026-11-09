
Microsoft uses URL-encoded SVGs, copying a class \
from an Icons website won't work by default.

Go to the Bootstrap Icons official site and find the icon you want.

Copy the raw SVG code.

~~Use a free online tool (like a URL Encoder or an SVG-to-CSS tool) \
to URL-encode the SVG snippet.~~

Make an svg file in the Images folder.

In the Path section add your SVG snippet: 
```
<path d="{-snippet here-}">
```
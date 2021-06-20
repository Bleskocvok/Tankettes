# Tančíky / Tankettes

![Icon](Icon.bmp)


- [Repository](https://github.com/Bleskocvok/Tankettes)

- [Additional info](https://youtu.be/dQw4w9WgXcQ)


## Notes:

1. File usage:

	1. `Serialization/Serializer.cs` - used for saving/loading
	game state

	1. (loading game assets in `Client/Renderer.cs`)

1. Multithreading usage:

	1. `Client/BackgroundSaver.cs` - uses a background task to save
	current game state

	1. `GameLogic/Terrain.cs` - procedural terrain generation uses parallel
	programming to perform indenpendent iterations of the perlin noise algorithm
	(it's much slower than the sequential version given the small input
	values... **but it's the thought that counts!**)

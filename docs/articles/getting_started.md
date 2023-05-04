# 1. Getting Started

Create a new folder and inside it, create a new file `main.lua`. You will need four functions, as defined below.

```lua
-- Called before the game is loaded with the EngineProperties object. 
-- The EngineProperties object can be modifed and must be returned to the engine.
function Steel.Preload(engineProperties)
	engineProperties.Width = 1000
	engineProperties.Height = 740
	
	return engineProperties
end

function Steel.Load()
	-- called when the game is initially loaded
end

function Steel.Update(dt)
	-- called once every frame 
end

function Steel.Render()
	-- called once every frame, for rendering.
end

```
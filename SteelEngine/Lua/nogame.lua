local numRectangles = 1000
local lerpPos = 0
local color = Color(255, 0,0)

local function testPerformance()
    for i = 1, numRectangles do
        lerpPos = Lerp(lerpPos, i / numRectangles, Time.GetDeltaTime() * 2)
        color.r = math.floor(lerpPos * 255)
        Draw.DrawRectangle(i, i, 10, 10, color)
    end
end

function Steel.Preload(ctx)
	ctx.Width = 500
	ctx.Height = 500
	ctx.Title = "Steel Engine - No Game!"
	ctx.Version = nil
	ctx.BackgroundColor = Color(130, 140, 220)
	
	return ctx
end

function Steel.Load()
	Draw.CreateFont("Default", "resources/fonts/Roboto-Regular.ttf", 70)
	
	-- Draw.CreateTexture("meme", "resources/textures/test.png")
end

function Steel.Update(dt)
	if (Input.GetKeyDown(KeyCode.Escape)) then
		Window.Close()
	end
end

function Steel.Render()
	-- testPerformance()

	Draw.DrawText("Hello, World!", "Default", 150, 150, Color(255, 0, 0))

	-- Draw.DrawRectangle(50, 50, 100, 100, Color(0, 255, 0, 255))

	-- Draw.DrawRectangle(0, 0, 100, 100, Color(255, 0, 0, 255))

	-- Draw.DrawRectangle(150, 0, 100, 100, Color(0, 0, 255, 255))

	-- Draw.DrawTexturedRectangle(100, 100, 250, 250, "meme", Color(255, 255, 255))
end

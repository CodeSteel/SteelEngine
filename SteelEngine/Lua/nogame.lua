function Steel.Preload(ctx)
	ctx.Width = 500
	ctx.Height = 500
	ctx.Title = "Steel Engine - No Game!"
	ctx.Version = nil
	ctx.BackgroundColor = Color(130, 140, 220)
	
	return ctx
end

function Steel.Load()
	--Time.CreateTimer("Cool Kids", 1, 0, function()
	--	print("HAHAHAHA")
		-- print(Time.TimerExists("Cool Kidsss"))
	--end)
end

function Steel.Update(dt)
	if (Input.GetKeyDown(KeyCode.Escape)) then
		Window.Close()
	end
end

local numRectangles = 1000
local lerpPos = 0
local color = Color(255, 0,0)
local startTime = Time.GetTime()

local function testPerformance()
    for i = 1, numRectangles do
        lerpPos = Lerp(lerpPos, i / numRectangles, Time.GetDeltaTime() * 2)
        color.r = math.floor(lerpPos * 255)
        Draw.DrawRectangle(i, i, 10, 10, color)
    end
end

function Steel.Render()
	testPerformance()

end

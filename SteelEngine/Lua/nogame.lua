function Steel.Preload(ctx)
	ctx.Width = 500
	ctx.Height = 500
	ctx.Title = "Steel Engine - No Game!"
	ctx.Version = nil
	ctx.BackgroundColor = Color(130, 140, 220)
	
	return ctx
end

function Steel.Load()
end

function Steel.Update(dt)
	if (Input.GetKeyDown(KeyCode.Escape)) then
		Window.Close()
	end
end

local lerpPos = 0

function Steel.Render()
	lerpPos = Lerp(0, Clamp(math.cos(Time.GetTime() * 2) * ScrW() + (ScrW() / 2), 100, ScrW() - 100), Time.GetTime() * 2)

	Draw.DrawRectangle(0, ScrH() * 0.5 - 50, ScrW(), 100, Color(30, 70, 200));
	Draw.DrawCircle(lerpPos, ScrH() * 0.5 - 50, 100, Color(200, 255, 50), 32)
end

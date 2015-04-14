------------------
-- Class NaviUI
-- 第一个测试组件的类
------------------
require("LuaComponent")
local base = LuaComponent -- 父类

------------ 自动生成区 ------------
local class = { mt = {} }
NaviUI = class -- 修改类名
class.mt.__index = class
if (base) then setmetatable(class, base.mt) end
------------ 自动生成区 ------------

function NaviUI:Start()
	local map = self.UnityComponent.transform:Find("Map")
	self.Map = map
	local levelButtons = {}
	self.LevelButtons = levelButtons
	levelButtons[1] = map:Find("Button"):GetComponent("LuaComponent").LuaObject
	print("Should Refresh")
	levelButtons[1]:Refresh({Id = 242, Stars = 2})
	
end

function NaviUI:OnQuitClick() -- 修改类名
    luanet.load_assembly('UnityEngine')
    local Application = luanet.import_type('UnityEngine.Application')

    -- local newGameObj = GameObject('NewObj')
    print("QUIT from LUA")
    Application.Quit()
end
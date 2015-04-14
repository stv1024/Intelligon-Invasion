------------------
-- Class LevelButton
-- 第一个测试组件的类
------------------
require("LuaComponent")
local base = LuaComponent -- 父类

------------ 自动生成区 ------------
local class = { mt = {} }
LevelButton = class -- 修改类名
class.mt.__index = class
if (base) then setmetatable(class, base.mt) end
------------ 自动生成区 ------------

function LevelButton:Awake()
	self.rads = {}
	for i=1,3 do
		self.rads[i] = self.UnityComponent.transform:Find("Rad"..i)
	end
end

function LevelButton:OnClick() -- 修改类名
    print("Click Level"..self.LevelId)
end

function LevelButton:Refresh(userLavelData)
	self.LevelId = userLavelData.Id
	print("Refresh OK "..self.LevelId)
end
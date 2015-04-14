------------------
-- Class TestComponent
-- 第一个测试组件的类
------------------
print("LC ran")
require("LuaComponent")
local base = LuaComponent -- 父类

------------ 自动生成区 ------------
local class = { mt = {} }
TestComponent = class -- 修改类名
class.mt.__index = class
if (base) then setmetatable(class, base.mt) end
------------ 自动生成区 ------------

function TestComponent:Awake() -- 修改类名
	print("Yeah! Hello world!")
	print("I got "..self.UnityComponent.gameObject.name)
end
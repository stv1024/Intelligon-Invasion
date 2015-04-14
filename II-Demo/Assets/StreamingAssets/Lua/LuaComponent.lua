------------------
-- Class LuaComponent
-- Substitute for Unity GameObject Class
------------------

luanet.load_assembly('UnityEngine')
Type = {}
Type.LuaComponent = luanet.import_type("UnityEngine.Component")
Type.LuaComponent = luanet.import_type("LuaComponent")
Type.LuaTable = luanet.import_type("LuaInterface.LuaTable")

local base = nil -- 父类

------------ 自动生成区 ------------
local class = { mt = {} }
LuaComponent = class
class.mt.__index = class
if (base) then setmetatable(class, base.mt) end

function LuaComponent:new(unityComponent)
	o = {UnityComponent = unityComponent}
	setmetatable(o, self)
	self.__index = self
	o.UnityComponent.LuaObject = o
end
------------ 自动生成区 ------------

-- function LuaComponent:GetUnityComponent()

            -- luanet.load_assembly('UnityEngine')
            -- local MonoBehaviour = luanet.import_type('UnityEngine.MonoBehaviour')

            -- local newGameObj = MonoBehaviour('NewObj')
            -- newGameObj:AddComponent('ParticleSystem')
	-- return self.gameObject
-- end

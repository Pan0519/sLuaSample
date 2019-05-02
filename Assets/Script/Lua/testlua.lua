import "UnityEngine"
if not UnityEngine.GameObject then
    error("Click Make/All to generate lua wrap file")
end

local class = {}

local arr = {1, 2, 3, 4, 5}

function main()
    print("lua--- run lua main function")
    local even = map(arr, function (n) return n * n end)
    print("Main Array_Req : ", array_req(even, {1, 4, 9, 16, 25}))
end

local dict = {
    one = "eni",
    two = "zw",
    three = "dri"
}

table.sort(arr, function(a, b) return a < b end)

function testFun()
    runDict()
end

function runDict()
    local iter = dict:GetEnumerator()
    
    while iter:MoveNext() do
        print("key : "..iter.Current.Key.."Value : "..iter.Current.Value)
    end
end

function grep(arr, f)
    local a = {}
    
    for i = 1, #arr do
        if f(arr[i]) then
            table.insert(a, arr[i])
        end
    end
    return a
end

function map(arr, f)
    local a = {}
    
    for i = 1, #arr do
        table.insert(a, f(arr[i]))
    end
    return a
end

function sum(b)
    -- body
    local s = 5
    
    s = s + b
    
    print(b.."+"..s)
    
    return s
end

function prime()
    
    local n = 1
    local out
    
    return function ()
        while true do
            n = n + 1
            local isPrime = true
            
            for i = 2, math.floor(math.sqrt(n)) do
                if n % i == 0 then
                    isPrime = false
                    break
                end
            end
            
            if isPrime then
                out = n
                break
            end
        end
        return out
        
    end
    
end

function array_req(a1, a2)
    
    if #a1 ~= a2 then
        return false
    end
    
    for i = 1, #a1 do
        if a1[i] ~= a2[i] then
            return false
        end
    end
    
    return true
end

function number()
    
    local i = -1
    
    return function ()
        i = i + 1
        return i
    end
    
end

function add(a, b)
    if type(a) == "string" or type(b) == "stirng" then
        return a..b
    else
        return a + b
    end
end

-- 在C#的Update函数中被调用
function class:Update()
    print(Time.deltaTime)
end


local mode = nil
local svs = ''
local svarying = ''
local sfs = ''

for l in io.lines(...) do
  if string.find(l, '//!vs') == 1 then
    mode = 'vs'
  elseif string.find(l, '//!varying') == 1 then
    mode = 'varying'
  elseif string.find(l, '//!fs') == 1 then
    mode = 'fs'
  else
    if mode == 'vs' then
      svs = svs .. l .. '\n'
    elseif mode == 'varying' then
      svarying = svarying .. l .. '\n'
    elseif mode == 'fs' then
      sfs = sfs .. l .. '\n'
    end
  end
end

if #svs == 0 then
  print('missing //!vs section in ', ...)
end
if #svarying == 0 then
  print('missing //!varying section in ', ...)
end
if #sfs == 0 then
  print('missing //!fs section in ', ...)
end
if #svs == 0 or #svarying == 0 or #sfs == 0 then error('missing section (unscx)') end

local function wo(path, what)
  local w = io.open(path, 'w+')
  w:write(what)
  w:flush()
  w:close()
end

local rpth = string.sub(..., #'shaders/src/'+1, -5)
wo('shaders/tmp/' .. rpth .. '.vs.sc', svs)
wo('shaders/tmp/' .. rpth .. '.varying.sc', svarying)
wo('shaders/tmp/' .. rpth .. '.fs.sc', sfs)
print('unscx ok')

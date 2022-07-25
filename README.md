# js4irc

js4irc brings the V8 Javascript engine to AdiIRC.

## Usage: $js.execScript(<javascript>)

eg. `$js.execScript(JSON.stringify(globalThis))` returns `{}`

`$js.error` should be checked after calling `$js.execScript`. If non-null, `$js.error` contains the human readable error message.

## Download

Download is available from https://github.com/realJoshByrnes/js4irc/releases/ and the files should be placed in your AdiIRC\Plugins folder.

## Example

````mrc
alias js {
  var %x $js.execScript($1-)
  if ($isid) return %x
  echo $color(info) -a -> $1-
  if ($js.error) msgbox $ifmatch
  else echo $color(info2) -a <- %x
}

````
Now simply typing /js as a command will work similarly to a JS console.
````mrc
/js Math.PI
````

-> Math.PI<br>
<- 3.14159265358979

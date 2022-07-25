# js4irc

js4irc brings the V8 Javascript engine to AdiIRC.

## Usage: $js.execScript(<javascript>)

eg. `$js.execScript(JSON.stringify(globalThis))` returns `{}`

`$js.error` should be checked after calling `$js.execScript`. If non-null, `$js.error` contains the human readable error message.

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

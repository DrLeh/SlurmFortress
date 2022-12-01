// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

let light = 'bootstrap';
let dark = 'bootstrap-dark';
let theme = dark;
function toggleTheme() {
  setTheme(light);
  setTheme(dark);
  if (theme == light)
    setTheme(dark);
  else
    setTheme(light);
}

function setTheme(theTheme) {
  theme = theTheme;
  document.body.className = theTheme;
  window.localStorage.setItem(themeKey, theme);
}

var themeKey = "theme";
function loadTheme() {
  let storedTheme = window.localStorage.getItem(themeKey);
  if (storedTheme)
    setTheme(storedTheme);
}
loadTheme();

// the main gameloop. Will be called 60 times per second by requestAnimationFrame
function gameLoop(timeStamp) {
  window.requestAnimationFrame(gameLoop);
  game.instance.invokeMethodAsync('GameLoop', timeStamp);//, game.canvases[0].width, game.canvases[0].height);
}

// will be called by blazor to initialize the game and register the game instance.
window.initGame = (instance) => {
  var canvasContainer = document.getElementById('canvasContainer');
  var canvases = canvasContainer.getElementsByTagName('canvas') || [];

  window.game = {
    instance: instance,
    canvases: canvases
  };
  window.requestAnimationFrame(gameLoop);
};
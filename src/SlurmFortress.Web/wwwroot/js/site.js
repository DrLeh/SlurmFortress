// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

let light = 'bootstrap';
let dark = 'bootstrap-dark';
let theme = dark;
function toggleTheme() {
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
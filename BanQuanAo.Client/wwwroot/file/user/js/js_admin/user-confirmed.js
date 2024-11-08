function block(id) {
  document.getElementById(id).style.backgroundColor = "#bd2130";
  document.getElementById(id).style.color = "white";
}
function unblock(id) {
  document.getElementById(id).style.backgroundColor = "#28a745";
  document.getElementById(id).style.color = "white";
}
function check_block(id) {
  var s = document.getElementById(id);
  if (s.value == "true") {
    block(id);
    s.setAttribute("value", "false");
    s.setAttribute("data-target", "#unblock-user");
  } else {
    unblock(id);
    s.setAttribute("value", "true");
    s.setAttribute("data-target", "#block-user");
  }
}
function remove_row(id) {

}

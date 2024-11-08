function click_confirmed(id) {
  var session = document.getElementById(id).getElementsByTagName("span");
  var cf = session[0].getElementsByTagName("label")[0];
  var deli = session[1].getElementsByTagName("label")[0];
  cf.setAttribute("class", "confirmed");
  $(cf).removeAttr("data-toggle");
  deli.setAttribute("class", "active");
  deli.setAttribute("data-toggle", "modal");
}
function click_delivered(id) {
  var session = document.getElementById(id).getElementsByTagName("span");

  var deli = session[1].getElementsByTagName("label")[0];
  $(deli).removeAttr("data-toggle");
  deli.setAttribute("class", "confirmed");
}
function click_cancel(id) {
  var session = document.getElementById(id).getElementsByTagName("span");
  var locationRow = parseInt(id[id.length - 1]);
  console.log(locationRow);
  var deli = session[2].getElementsByTagName("label")[0];
  var row = document.getElementById("content-table").getElementsByTagName("tr")[
    locationRow - 1
  ];
  row.remove();
}
function setID_confirm() {
  var rows = document
    .getElementById("content-table")
    .getElementsByTagName("tr");
  var length = rows.length;
  for (let i = 0; i < length; i++) {
    var column = rows[i].getElementsByTagName("td")[5];
    var idsession = "confirm-session-" + (i + 1);
    column.setAttribute("id", idsession);
    var spans = column.getElementsByTagName("span");
    var lenthSpan = spans.length;
    confirm_span(spans[0], i + 1, idsession);
    delivered_span(spans[1], i + 1, idsession);
    cancel_span(spans[2], i + 1, idsession);
  }
}
function confirm_span(el, number, idsession) {
  var label = el.getElementsByTagName("label")[0];
  label.setAttribute("data-target", "#pop-confirmed-" + number);
  var pop = el.getElementsByTagName("div")[0];
  pop.setAttribute("id", "pop-confirmed-" + number);
  var button = el.getElementsByTagName("button")[2];
  button.onclick = function() {
    click_confirmed(idsession);
  };
}
function delivered_span(el, number, idsession) {
  var label = el.getElementsByTagName("label")[0];
  label.setAttribute("data-target", "#pop-delivered-" + number);
  var pop = el.getElementsByTagName("div")[0];
  pop.setAttribute("id", "pop-delivered-" + number);
  var button = el.getElementsByTagName("button")[2];
  button.onclick = function() {
    click_delivered(idsession);
  };
}
function cancel_span(el, number, idsession) {
  var label = el.getElementsByTagName("label")[0];
  label.setAttribute("data-target", "#pop-cancel-" + number);
  var pop = el.getElementsByTagName("div")[0];
  pop.setAttribute("id", "pop-cancel-" + number);
  var button = el.getElementsByTagName("button")[2];
  button.onclick = function() {
    click_cancel(idsession);
    setID_confirm();
  };
}
setID_confirm();

var move = 1;
function next_page() {
  var lineNo = parseInt(document.getElementById("show").value);
  var length = parseInt(
    document.getElementById("content-table").getElementsByTagName("tr").length
  );
  var numberPage = Math.ceil(length / lineNo);
  console.log(move);
  if (move < numberPage) {
    click_page(move + 1);
  }
  console.log(move);
}
function previous_page() {
  console.log(move);
  if (move > 1) {
    click_page(move - 1);
  }
  console.log(move);
}
function select_page() {
  var lineNo = parseInt(document.getElementById("show").value);
  var length = parseInt(
    document.getElementById("content-table").getElementsByTagName("tr").length
  );
  // So luong page hien tai it hon so luong page yeu cau
  if (lineNo >= length) {
    create_page(1);
    click_page(1);
  } else {
    var numberPage = Math.ceil(length / lineNo);
    create_page(numberPage);
    click_page(1);
  }
}
select_page();
// for (let i = 1; i <= numberPage; i++) {
//     for (let j = i * 10 - 10; j < i * 10; j++) {

//     }
//   }
function click_page(tag) {
  var lineNo = parseInt(document.getElementById("show").value);
  var arr = [];
  move = tag;
  console.log(move);
  var length = parseInt(
    document.getElementById("content-table").getElementsByTagName("tr").length
  );
  if (length > tag * lineNo) {
    for (let i = tag * lineNo - lineNo; i < tag * lineNo; i++) {
      arr.push(i);
    }
    show_line(arr);
  } else {
    var leo = length - (tag - 1) * lineNo;
    for (let i = 0; i < leo; i++) {
      arr.push((tag - 1) * lineNo + i);
    }
    show_line(arr);
  }
  var page = document.getElementById("page-number");
  var btns = page.getElementsByTagName("button");
  for (let i = 0; i < btns.length; i++) {
    var x = btns[i].id;
    if (i + 1 != tag) {
      btns[i].setAttribute("class", "");
    } else {
      btns[i].setAttribute("class", "active");
    }
  }
}

function show_line(arr) {
  var lines = document
    .getElementById("content-table")
    .getElementsByTagName("tr");
  var s = lines.length;
  for (let j = 0; j < s; j++) {
    for (let i = 0; i < arr.length; i++) {
      if (j != arr[i]) {
        lines[j].style.display = "none";
      }
      lines[arr[i]].style.display = "table-row";
    }
  }
}
function create_page(number) {
  var page = document.getElementById("page-number");
  var remove = page.getElementsByTagName("button");
  var s = remove.length;
  for (let i = 0; i < s; i++) {
    remove[0].remove();
  }
  for (let i = 0; i < number; i++) {
    var btn = document.createElement("button");
    page.appendChild(btn);
    btn.onclick = function () {
      click_page(i + 1);
    };
    btn.innerHTML = i + 1;
  }
}


/// thêm màu 



function themMau() {
  /// lấy số lượng form hiện tại
  var current = document.getElementById("btnAddColor").name;
  n = parseInt(current) + 1;


  $('#color').append(' <div  class="row boder-color">'
    + '<div class="col-4 space-top " id="chooseColor" name="1">'
    + '<h5 class="spacing_form">Màu sắc</h5>'
    + '<div class="input-group mb-2 ">'
    + '  <div class="input-group-prepend">'
    + '    <div class="input-group-text"><i class="fas fa-mobile-alt"></i></div>'
    + '  </div>'
    + '  <select name="" class="form-control w ">'
    + '    <option value="">Đen</option>'
    + '    <option value="">Trắng</option>'
    + '    <option  value="">Đỏ</option>'
    + '    <option value="">Xanh</option>'
    + '    <option value="">Hồng</option>'
    + '    <option value="">Lam</option>'
    + '  </select>'
    + '</div>'
    + '</div>'

    + '<div class="col-4 space-top">'
    + '<h5 class="spacing_form ">URL hình ảnh</h5>'
    + '<input class="mt-2" type="file" accept=".jpg,.png,.jpge">'
    + '</div>'

    + '<div class="col-4 space-top">'
    + '<h5 class="spacing_form">Số lượng</h5>'
    + '<div class="input-group mb-2">'
    + '  <div class="input-group-prepend">'
    + '    <div class="input-group-text"><i class="fas fa-tag "></i></div>'
    + '  </div>'
    + '  <input type="number" class="form-control py-4" value="" placeholder="Nhập số lượng">'
    + '</div>'
    + '</div>'
    + '</div>');
  var textnode = document.createTextNode("Demo" + n);
  node.appendChild(textnode);
  document.getElementById("addColorForm").appendChild(node);

  ///cập nhật lại số lượng form
  document.getElementById("btnAddColor").name = parseInt(current) + 1;
}
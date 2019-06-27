
var bookDataFromLocalStorage = [];

$(function(){
    loadBookData();
    var data = [
        {text:"資料庫",value:"database"},
        {text:"網際網路",value:"internet"},
        {text:"應用系統整合",value:"system"},
        {text:"家庭保健",value:"home"},
        {text:"語言",value:"language"}
    ]
    $("#book_category").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: onChange
    });
    $("#bought_datepicker").kendoDatePicker();
    $("#book_grid").kendoGrid({
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    fields: {
                        BookId: {type:"int"},
                        BookName: { type: "string" },
                        BookCategory: { type: "string" },
                        BookAuthor: { type: "string" },
                        BookBoughtDate: { type: "string" }
                    }
                }
            },
            pageSize: 20,
        },
        toolbar: kendo.template("<div class='book-grid-toolbar'><input class='book-grid-search' placeholder='我想要找......' type='text'></input></div>"),
        height: 550,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookId", title: "書籍編號",width:"10%"},
            { field: "BookName", title: "書籍名稱", width: "50%" },
            { field: "BookCategory", title: "書籍種類", width: "10%" },
            { field: "BookAuthor", title: "作者", width: "15%" },
            { field: "BookBoughtDate", title: "購買日期", width: "15%" },
            { command: { text: "刪除", click: deleteBook }, title: " ", width: "120px" }
        ]
        
    });
})

function loadBookData(){
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    if(bookDataFromLocalStorage == null){
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData",JSON.stringify(bookDataFromLocalStorage));
    }
}

function onChange(){
    var value = $("#book_category").val() ;
    $(".book-image").attr("src","image/"+value+".jpg")
    /*if(value==1) 
    {
        $(".book-image").attr("src","image/database.jpg")
    }
    else if(value==2) 
    {
        $(".book-image").attr("src","image/internet.jpg")
    }
    else if(value==3) 
    {
        $(".book-image").attr("src","image/system.jpg")
    }
    else if(value==4) 
    {
        $(".book-image").attr("src","image/home.jpg")
    }
    else if(value==5) 
    {
        $(".book-image").attr("src","image/language.jpg")
    }
    */
}
  
function deleteBook(options){
    
    var grid = $("#book_grid").data("kendoGrid");
    var dataItem = grid.dataItem($(options.currentTarget).closest("tr"));
    console.log(dataItem);
    var localData= JSON.parse(localStorage.getItem("bookData"));
    console.log("ready_delete");
    for(var i=0;i<localData.length;i++){
        console.log(options.BookId);
        if(localData[i].BookId==dataItem.BookId)
        {
            console.log("find");
            localData.splice(i,1);
            break;
        }

    }
    localStorage["bookData"]=JSON.stringify(localData);
    location.reload();
}

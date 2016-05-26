var productRecommendClick = function (productId, value) {
    layer.open({
        type: 2,
        title: "推荐单品",
        shadeClose: true,
        shade: 0.3,
        area: ["420px", "360px"],
        content: "/product/recommend/" + productId
    });
}

var productCollectClick = function (obj, id, value) {
    $.post("/product/" + $(obj).attr("id"), { productId: id, value: value }, function (result) {
        if (value) {
            $(obj).addClass("selected");
            $(obj).attr("title", "取消" + $(obj).attr("title"));
            $(obj).removeAttr("onclick");
            $(obj).off("click").on("click", function () {
                productCollectClick(obj, id, false);
            });
        } else {
            $(obj).removeClass("selected");
            $(obj).attr("title", $(obj).attr("title").replace("取消", ""));
            $(obj).removeAttr("onclick");
            $(obj).off("click").on("click", function () {
                productCollectClick(obj, id, true);
            });
        }

        layer.msg(result);
    });
}

var productDeleteClick = function (productId) {
    layer.confirm("Are you sure you want to delete？", {
        btn: ["Yes", "Cancel"] //按钮
    }, function () {
        $.post("/product/delete", { productId: productId }, function (result) {
            layer.msg(result);
        });
    });
}

var productGoup = function (productId) {
    $.post("/product/goup", { productId: productId }, function (result) {
        layer.msg(result);
    });
}

var rediretTo = function (productId) {
    layer.open({
        type: 2,
        title: "正在进入商品详情页面...",
        shadeClose: true,
        shade: 0.3,
        area: ["300px", "260px"],
        content: "/product/redirect/" + productId
    });
}

var skipTo = function (){
    var isSkip = $("#skip").val();
    if (isSkip === "0") {
        if ($("#writeable_iframe_0").contents().find("a").length > 0) {
            var href = $("#writeable_iframe_0").contents().find("a").attr("href");
            parent.location.href = href;
            $("#skip").val("1");
        }
        var time = parseInt($("#time_ico").val());
        if (time > 25) {
            var itemId = $("#item_id").val();
            parent.location.href = "http://item.taobao.com/item.htm?id=" + itemId;
            $("#skip").val("1");
        } else {
            $("#time_ico").val(time + 1);
        }
    }
}

var setCatSelectedCssForProductList = function () {
    if (window.location.href.indexOf("product/list") > -1) {
        var catId = $("#hdnCatId").val();
        $(".js-product-cat ul li a").each(function () {
            if ($(this).attr("id") == catId) {
                $(this).addClass("selected");
            }
        });
    }
}

var blockRedirectUI = function () {
    $.blockUI({
        message: "<div><img src='/image/bigloading.gif' style='width:300px;' /></div>",
        css: { "width": "100%", "border": "0px;", "top": "0", "left":0, "padding": "0px;", "margin": "0px;" },
        timeout: 6000
    });
}
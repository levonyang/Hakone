//收藏店铺，推荐店铺，设置热门店铺，设置精选店铺
var shopActionClick = function (obj, shopId, value) {
    if (value) {
        layer.prompt({ title: $(obj).attr("title") + "，想说点啥？ （可留空）", formType: 2 }, function (comment, index, elem) {
            postAction(obj, shopId, comment, value);
        });
    } else {
        postAction(obj, shopId, "", value);
    }
}

var postAction = function (obj, shopId, comment, value) {
    $.post("/shop/" + $(obj).attr("id"), { shopId: shopId, comment: comment, value: value }, function (result) {
        if ($(obj).attr("id") != "recommendshop") {
            if (value) {
                $(obj).addClass("selected");
                $(obj).attr("title", "取消" + $(obj).attr("title"));
                $(obj).removeAttr("onclick");
                $(obj).off("click").on("click", function() {
                    shopActionClick(obj, shopId, false);
                });
            } else {
                $(obj).removeClass("selected");
                $(obj).attr("title", $(obj).attr("title").replace("取消", ""));
                $(obj).removeAttr("onclick");
                $(obj).off("click").on("click", function() {
                    shopActionClick(obj, shopId, true);
                });
            }
        }

        layer.msg(result);
    });
}

var shopDeleteClick = function (shopId) {
    layer.confirm("Are you sure you want to delete the shop？", {
        btn: ["Yes", "Cancel"] //按钮
    }, function () {
        $.post("/shop/delete", { shopId: shopId }, function (result) {
            layer.msg(result);
        });
    });

}
var shopEditClick = function (shopId) {
    layer.open({
        type: 2,
        title: "编辑店铺",
        shadeClose: true,
        shade: 0.3,
        area: ["420px", "550px"],
        content: "/shop/edit/" + shopId
    });
}
var shopAddClick = function () {
    layer.open({
        type: 2,
        title: "添加店铺",
        shadeClose: true,
        shade: 0.3,
        area: ["420px", "560px"],
        content: "/shop/create"
    });
}

var shopProductToggleComment = function () {
    $(".shop-product-nav .shop-product-nav-product").click(function () {
        $(".shop-product .shop-product-orderby").show();
        $(".product-list-shop").show();
        $(".shop-product-form-comment").hide();
        $(".shop-comment-list").hide();
        $(this).addClass("selected");
        $(".shop-product-nav .shop-product-nav-comment").removeClass("selected");
    });
    $(".shop-product-nav .shop-product-nav-comment").click(function () {
        $(".shop-product .shop-product-orderby").hide();
        $(".product-list-shop").hide();
        $(".shop-product-form-comment").show();
        $(".shop-comment-list").show();
        $(this).addClass("selected");
        $(".shop-product-nav .shop-product-nav-product").removeClass("selected");
    });
}

var reft = function (shopId) {
    $.post("/shop/reft", { shopId: shopId }, function (result) {
        layer.msg(result);
    });
}

var rfsview = function () {
    $.post("/shop/rfsview", function (result) {
        layer.msg(result);
    });
}

var postShopComment =function() {
    $("#shop-comment-post").on("submit", function(e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr("action"),
            type: $(this).attr("method"),
            data: $(this).serialize(),
            success: function(result) {
                $(".shop-comment-list").html();
                $(".shop-comment-list").html(result);
            }
        });
    });
}
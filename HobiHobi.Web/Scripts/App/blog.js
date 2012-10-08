function TestController($scope) {
    $scope.newPost = function (post) {
        console.log(post.content);
    };
}
function BlogPostController($scope) {
    $scope.master = {
    };
    $scope.reset = function () {
        $scope.post = angular.copy($scope.master);
    };
    $scope.newPost = function (post) {
        var content = post.content;
        var activeTab = $('#feed_tabs li.active a');
        var id = activeTab.data('id');
        var doc = {
            feedId: id,
            content: content,
            link: null
        };
        var link = post.link;
        var json = JSON.stringify(doc);
        $scope.reset();
        alert(json);
    };
}
function countChar(val) {
    var len = val.value.length;
    if(len >= 280) {
        val.value = val.value.substring(0, 280);
    } else {
        if(len >= 140) {
            $('#post_content_count').text("* " + (279 - len));
        } else {
            $('#post_content_count').text(280 - len);
        }
    }
}
$(function () {
    var firstTab = $('#feed_tabs li:first');
    if(firstTab == null) {
        return;
    }
    firstTab.attr('class', 'active');
});
function load(feedId) {
    $.get('/manage/blog/getposts/?feedId=' + feedId, function (payload) {
        if(payload.Data.length == 0) {
            $('#posts').html('');
        } else {
            var template = $('#tmpl-posts').html();
            var compiled = _.template(template, {
                posts: payload.Data
            });
            $('#posts').html(compiled);
        }
    });
}
function inform(msg, target) {
    if (typeof target === "undefined") { target = undefined; }
    if(target === undefined) {
        $('#message').removeClass().addClass('alert alert-success').html(msg).show().fadeOut(3000);
    } else {
        $(target).removeClass().addClass('alert alert-success').html(msg).show().fadeOut(3000);
    }
}
function alarm(msg, target) {
    if(target === undefined) {
        $('#message').removeClass().addClass('alert alert-error').html(msg).show().fadeOut(10000);
    } else {
        $(target).removeClass().addClass('alert alert-error').html(msg).show().fadeOut(10000);
    }
}

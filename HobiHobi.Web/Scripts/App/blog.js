var blogModule = angular.module('blogModule', []);
blogModule.run(function ($rootScope) {
    $rootScope.$on('success-message', function (event, args) {
        args.type = "success";
        $rootScope.$broadcast('show-message', args);
    });
    $rootScope.$on('error-message', function (event, args) {
        args.type = "error";
        $rootScope.$broadcast('show-message', args);
    });
});
function PostController($scope, $q) {
    $scope.master = {
    };
    $scope.newPost = function (post) {
        var activeTab = $('#feed_tabs li.active a');
        var id = activeTab.data('id');
        if(post === undefined) {
            $scope.$emit('error-message', {
                message: 'content is required'
            });
            return;
        }
        var doc = {
            feedId: id,
            content: post.content,
            link: null
        };
        if(angular.isDefined(post.link)) {
            doc.link = post.link;
        }
        $scope.post = angular.copy($scope.master);
        var deferred = $q.defer();
        var json = JSON.stringify(doc);
        $.ajax('/manage/blog/createpost', {
            data: json,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(function (payload) {
            var template = $('#tmpl-single-post').html();
            var compiled = _.template(template, {
                post: payload.Data
            });
            $('#posts').prepend(compiled);
            $scope.$apply(function () {
                deferred.resolve($scope.$emit('success-message', {
                    message: "Your post is successfully added"
                }));
            });
        });
    };
}
function MessageController($scope) {
    $scope.$on('show-message', function (event, args) {
        $scope.message = args.message;
        if(args.type === "success") {
            $scope.type = "alert alert-success";
        } else {
            $scope.type = "alert alert-error";
        }
    });
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
    var firstTabUrl = firstTab.children(':first');
    $('#feed_tab_link').attr('href', '/f/' + firstTabUrl.data('url'));
    load(firstTabUrl.data('id'));
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
$('a[data-toggle="tab"]').on('shown', function (e) {
    var id = $(e.target).data('id');
    $('#feed_tab_link').attr('href', '/f/' + $(e.target).data('url'));
    load(id);
});
$('#feed_new').click(function () {
    $('#feed_create').modal();
});
$('#save_new_feed').click(function () {
    var title = $.trim($('#feed_title').val());
    if(title.length < 6) {
        alarm('Title must be at least 10 characters long', '#message_popup');
        return;
    }
    var description = $.trim($('#feed_description').val());
    if(description.length == 0) {
        alarm('Description is required', '#message_popup');
        return;
    }
    var doc = {
        blogId: $('#feed_blog_id').val(),
        title: title,
        description: description
    };
    var json = JSON.stringify(doc);
    $.ajax('/manage/blog/createfeed', {
        data: json,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'
    }).done(function (payload) {
        if(payload.StatusCode != 200) {
            alarm(payload.StatusMessage + ":" + payload.ErrorDetails, '#message_popup');
        } else {
            document.location.reload(true);
        }
    });
});
function postFormReset() {
    $('#post_content').val('');
    $('#post_link').val('');
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

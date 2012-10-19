var MessageType;
(function (MessageType) {
    MessageType._map = [];
    MessageType._map[0] = "ERROR";
    MessageType.ERROR = 0;
    MessageType._map[1] = "INFO";
    MessageType.INFO = 1;
    MessageType._map[2] = "SUCCESS";
    MessageType.SUCCESS = 2;
    MessageType._map[3] = "WARNING";
    MessageType.WARNING = 3;
})(MessageType || (MessageType = {}));

var UserMessage = (function () {
    function UserMessage(Message, Type) {
        this.Message = Message;
        this.Type = Type;
    }
    return UserMessage;
})();
var blogModule = angular.module('blogModule', []);
blogModule.run(function ($rootScope) {
    $rootScope.$on('data-posts', function (event, args) {
        $rootScope.$broadcast('list-posts', args);
    });
    $rootScope.$on('data-single-post', function (event, args) {
        $rootScope.$broadcast('list-append-post', args);
    });
});
var notification = [
    '$window', 
    function (win) {
        return function (args) {
            var msg = angular.element('#user-message');
            switch(args.Type) {
                case MessageType.ERROR: {
                    msg.removeClass().addClass('alert alert-error').text(args.Message);
                    break;

                }
                case MessageType.INFO: {
                    msg.removeClass().addClass('alert alert-info').text(args.Message);
                    break;

                }
                case MessageType.SUCCESS: {
                    msg.removeClass().addClass('alert alert-success').text(args.Message);
                    break;

                }
                case MessageType.WARNING: {
                    msg.removeClass().addClass('alert').text(args.Message);

                }
            }
        }
    }];
blogModule.factory('notification', notification);
blogModule.directive('showonhoverparent', function () {
    return {
        link: function (scope, element, attrs) {
            element.parent().bind('mouseenter', function () {
                element.show();
                element.css('cursor', 'pointer');
            });
            element.parent().bind('mouseleave', function () {
                element.hide();
                element.css('cursor', 'default');
            });
        }
    };
});
var FeedSettingsController = (function () {
    function FeedSettingsController($scope, notification) {
        $scope.say = function () {
            notification(new UserMessage("Hello world", MessageType.INFO));
        };
    }
    return FeedSettingsController;
})();
var PostController = (function () {
    function PostController($scope, $q, notification) {
        $scope.master = {
        };
        $scope.newPost = function (post) {
            var activeTab = $('#feed_tabs li.active a');
            var id = activeTab.data('id');
            if(post === undefined) {
                notification(new UserMessage("Content is required", MessageType.ERROR));
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
                $scope.$apply(function () {
                    deferred.resolve(notification(new UserMessage("Your post is successfully added", MessageType.SUCCESS)));
                    deferred.resolve($scope.$emit('data-single-post', [
                        payload.Data
                    ]));
                });
            });
        };
    }
    return PostController;
})();
var MessageController = (function () {
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
    return MessageController;
})();
var PostListController = (function () {
    function PostListController($scope, $q, notification) {
        $scope.posts = [];
        $scope.$on('list-posts', function (event, args) {
            $scope.posts = args.posts;
        });
        $scope.showLink = function (link) {
            if(link != null) {
                return '<a href=" ' + link + '"><b class="icon-zoom-in"></b></a>';
            } else {
                return '';
            }
        };
        $scope.$on('list-append-post', function (event, args) {
            $scope.posts = args.concat($scope.posts);
        });
        $scope.showActions = function (e) {
        };
        $scope.deletePost = function (e) {
            var el = angular.element(e.srcElement);
            el.removeClass('icon-remove');
            el.children().show();
        };
        $scope.confirmDeletion = function (e, post, confirm) {
            var el = angular.element(e.srcElement);
            if(confirm) {
                var doc = {
                    feedId: post.FeedId,
                    postId: post.Id
                };
                var deferred = $q.defer();
                var json = JSON.stringify(doc);
                $.ajax('/manage/blog/deletepost', {
                    data: json,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                }).done(function (payload) {
                    $scope.$apply(function () {
                        if(payload.StatusCode !== 200) {
                            notification(new UserMessage(payload.ErrorDetails, MessageType.ERROR));
                        } else {
                            deferred.resolve(el.parent().parent().remove());
                        }
                    });
                });
            } else {
                el.parent().addClass('icon-remove').children().hide();
            }
        };
    }
    return PostListController;
})();
var TabsController = (function () {
    function TabsController($scope, $q, $rootElement) {
function load(feedId) {
            var deferred = $q.defer();
            $.get('/manage/blog/getposts/?feedId=' + feedId, function (payload) {
                if(payload.Data.length == 0) {
                    $('#posts').html('');
                } else {
                    $scope.$apply(function () {
                        deferred.resolve($scope.$emit('data-posts', {
                            posts: payload.Data
                        }));
                    });
                }
            });
        }
        $rootElement.ready(function () {
            var firstTab = $('#feed_tabs li:first');
            if(firstTab == null) {
                return;
            }
            firstTab.attr('class', 'active');
            var firstTabUrl = firstTab.children(':first');
            var feedId = firstTabUrl.data('id');
            load(feedId);
        });
        $scope.load = function (e) {
            var el = angular.element(e.srcElement);
            var feedId = el.data('id');
            $('#feed_tab_link').attr('href', '/f/' + el.data('url'));
            load(feedId);
        };
        $scope.showNewFeedDialog = function () {
            $('#feed_create').modal();
        };
    }
    return TabsController;
})();
var FeedController = (function () {
    function FeedController($scope) {
        $scope.newFeed = function (feed) {
            if(feed == undefined) {
                alert('feed must exists');
                return;
            }
            if(!angular.isDefined(feed.description)) {
                alert('description is required');
                return;
            }
            var doc = {
                blogId: $('#feed_blog_id').val(),
                title: feed.title,
                description: feed.description
            };
            var json = JSON.stringify(doc);
            $.ajax('/manage/blog/createfeed', {
                data: json,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            }).done(function (payload) {
                if(payload.StatusCode != 200) {
                } else {
                    document.location.reload(true);
                }
            });
        };
    }
    return FeedController;
})();
function countChar(val) {
    var len = val.value.length;
    if(len >= 280) {
        val.value = val.value.substring(0, 280);
    } else {
        if(len >= 140) {
            $('#post_content_count').text("* " + (279 - len));
        } else {
            $('#post_content_count').text((280 - len) + "");
        }
    }
}

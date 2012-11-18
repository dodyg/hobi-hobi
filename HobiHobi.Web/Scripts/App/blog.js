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
            });
            element.parent().bind('mouseleave', function () {
                element.hide();
            });
        }
    };
});
blogModule.directive('alert', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch('alertType', function (val) {
                if(angular.isDefined(val)) {
                    element.addClass('alert ' + val);
                }
            }, true);
        }
    };
});
var AuthenticationController = (function () {
    function AuthenticationController($rootElement, $scope, $http, notification) {
        $rootElement.ready(function () {
            $http.get('/manage/identity/isauthenticated').success(function (payload) {
                $scope.is_authenticated = payload.Data;
                if(!payload.Data) {
                    $scope.alertType = "alert-info";
                    $scope.message = '';
                    $scope.loggedIn = false;
                } else {
                    $scope.alertType = "alert-info";
                    $scope.loggedIn = true;
                    $scope.message = "you are logged in";
                }
            });
        });
        $scope.showLoginPanel = function () {
            angular.element('#login_to_system').modal();
        };
    }
    return AuthenticationController;
})();
var LoginController = (function () {
    function LoginController($rootElement, $scope, $http, notification) {
        $scope.tryLogin = function (loginInfo) {
            if(loginInfo == undefined) {
                alert('Please enter the values');
                return;
            }
            if(!angular.isDefined(loginInfo.username)) {
                alert('username is required');
                return;
            }
            if(!angular.isDefined(loginInfo.password)) {
                alert('password is required');
                return;
            }
            var doc = {
                Email: loginInfo.username,
                Password: loginInfo.password
            };
            var json = JSON.stringify(doc);
            $http.post('/manage/identity/authenticate', json).success(function (payload) {
                if(payload.StatusCode == 200) {
                    alert('success');
                } else {
                    var errors = JSON.parse(payload.ErrorDetails);
                    angular.forEach(errors.Properties, function (v, k) {
                        alert(v.Errors[0]);
                    });
                }
            });
        };
    }
    return LoginController;
})();
var PostController = (function () {
    function PostController($scope, $q, $http, notification) {
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
            var json = JSON.stringify(doc);
            $http.post('/manage/blog/createpost', json).success(function (payload) {
                notification(new UserMessage("Your post is successfully added", MessageType.SUCCESS));
                $scope.$emit('data-single-post', [
                    payload.Data
                ]);
            });
        };
    }
    return PostController;
})();
var PostListController = (function () {
    function PostListController($scope, $q, $http, notification) {
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
                var json = JSON.stringify(doc);
                $http.post('/manage/blog/deletepost', json).success(function (payload) {
                    if(payload.StatusCode !== 200) {
                        notification(new UserMessage(payload.ErrorDetails, MessageType.ERROR));
                    } else {
                        el.parent().parent().remove();
                    }
                });
            } else {
                el.parent().addClass('icon-remove').children().hide();
            }
        };
    }
    return PostListController;
})();
var TabsController = (function () {
    function TabsController($scope, $q, $rootElement, $route, $http) {
function load(feedId) {
            $http.get('/manage/blog/getposts/?feedId=' + feedId).success(function (payload) {
                $scope.$emit('data-posts', {
                    posts: payload.Data
                });
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
            $scope.feedTabLink = '/f/' + firstTabUrl.data('url');
            $scope.feedId = feedId;
            load(feedId);
        });
        $scope.load = function (e) {
            var el = angular.element(e.srcElement);
            var feedId = el.data('id');
            $scope.feedTabLink = '/f/' + el.data('url');
            $scope.feedId = feedId;
            load(feedId);
        };
        $scope.showNewFeedDialog = function () {
            $('#feed_create').modal();
        };
        $scope.deleteFeed = function (e) {
            var el = angular.element(e.srcElement);
            el.removeClass('icon-remove');
            el.children().show();
        };
        $scope.confirmDeletion = function (e, feedId, confirm) {
            var el = angular.element(e.srcElement);
            if(confirm) {
                var doc = {
                    feedId: feedId
                };
                var json = JSON.stringify(doc);
                $http.post('/manage/blog/deletefeed', json).success(function (payload) {
                    if(payload.StatusCode != 200) {
                    } else {
                        document.location.reload(true);
                    }
                });
            } else {
                el.parent().addClass('icon-remove').children().hide();
            }
        };
    }
    return TabsController;
})();
var FeedController = (function () {
    function FeedController($scope, $http) {
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
                blogId: angular.element('#feed_blog_id').text(),
                title: feed.title,
                description: feed.description
            };
            var json = JSON.stringify(doc);
            $http.post('/manage/blog/createfeed', json).success(function (payload) {
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

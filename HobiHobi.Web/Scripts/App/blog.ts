///<reference path='lib.ts' />

declare var $;
declare var _;
declare var angular;

enum MessageType {
    ERROR,
    INFO,
    SUCCESS,
    WARNING
}

class UserMessage {
    constructor (public Message: string, public Type: MessageType) { }
}

/* module configuration */
var app = angular.module('MICRO_BLOG', []);
app.run(function ($rootScope) {
    $rootScope.$on('data-posts', function (event, args) {
        $rootScope.$broadcast('list-posts', args);
    });

    $rootScope.$on('data-single-post', function (event, args) {
        $rootScope.$broadcast('list-append-post', args);
    });
});

var notificationService: any[] = ['$window', function (win) {
    return function (args: UserMessage) {
        var msg = angular.element('#user_message');
        switch (args.Type) {
            case MessageType.ERROR : msg.removeClass().addClass('alert alert-error').html(args.Message);
                break;
            case MessageType.INFO : msg.removeClass().addClass('alert alert-info').html(args.Message);
                break;
            case MessageType.SUCCESS : msg.removeClass().addClass('alert alert-success').html(args.Message);
                break;
            case MessageType.WARNING: msg.removeClass().addClass('alert').html(args.Message);
        }
    };
}];

app.factory('notification', notificationService);

app.directive('showonhoverparent',
   function() {
      return {
         link : function(scope, element, attrs) {
            element.parent().bind('mouseenter', function() {
                element.show();
            });
            element.parent().bind('mouseleave', function() {
                 element.hide();
           });
       }
   };
});

app.directive('alert',
    function () {
        return {
            restrict : 'A',
            link: function (scope, element, attrs) {
                scope.$watch('alertType', function (val) {
                    if (angular.isDefined(val)) {
                        element.addClass('alert ' + val);
                    }
                }, true);
            }
        };
    });

var route: any[] = ['$routeProvider', function ($routes) {
}];

app.config(route);

/* Start controller section */

class AuthenticationController {
    constructor ($rootElement, $scope, $http, notification: (UserMessage) => void) {
        $rootElement.ready(function () {
            var isSecure = angular.element('#feed_blog_is_secure').text() == "True";
            
            if (!isSecure) {
                var secureLink = angular.element('#feed_blog_secure_url').text();
                notification(new UserMessage (
                    'You are accessing this page in a non secure way. Please click <a href="' + secureLink + '">here</a> to access it securely.', MessageType.ERROR));
            }
            
            $http.get('/manage/identity/isauthenticated').success(function (payload) {
                $scope.is_authenticated = payload.Data;
                if (!payload.Data) {
                    $scope.alertType = "alert-info";
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
        }

        $scope.tryLogout = function () {
            var json = JSON.stringify({})
            $http.post('/manage/identity/logout', json).success(function (payload) {
                if (payload.StatusCode == 200) {
                    $scope.loggedIn = false;
                }
            });
        }
    }
}

class LoginController {
    constructor ($rootElement, $scope, $http, notification) {
        $scope.tryLogin = function (loginInfo) {
            if (loginInfo == undefined) {
                alert('Please enter the values');
                return;
            }

            if (!angular.isDefined(loginInfo.username)) {
                alert('username is required');
                return;
            }
            if (!angular.isDefined(loginInfo.password)) {
                alert('password is required');
                return;
            }

            var doc = {
                Email: loginInfo.username,
                Password: loginInfo.password
            };

            var json = JSON.stringify(doc);

            $http.post('/manage/identity/authenticate', json).success(function (payload) {
                if (payload.StatusCode == 200) {
                    $scope.$parent.loggedIn = true
                    angular.element('#login_to_system').modal('hide');
                } else {
                    var errors = JSON.parse(payload.ErrorDetails);
                    angular.forEach(errors.Properties, function (v, k) {
                        alert(v.Errors[0]);
                    });
                }
            });
 
        }
    }
}

class PostController {
    constructor ($scope, $q, $http, notification) {
        $scope.master = {};
        $scope.newPost = function (post) {

            var activeTab = $('#feed_tabs li.active a');
            var id = activeTab.data('id');

            if (post === undefined) {
                notification(new UserMessage("Content is required", MessageType.ERROR));
                return;
            }

            var doc = {
                feedId: id,
                content: post.content,
                link: null
            };

            if (angular.isDefined(post.link))
                doc.link = post.link;

            $scope.post = angular.copy($scope.master);
            var json = JSON.stringify(doc);

            $http.post('/manage/blog/createpost', json).success(function (payload) {
                    notification(new UserMessage("Your post is successfully added", MessageType.SUCCESS));
                    $scope.$emit('data-single-post', [payload.Data]);
            });
        }//end of $scope.newPost
    }
}

class PostListController {
    constructor ($scope, $q, $http, notification) {
        $scope.posts = []

        $scope.$on('list-posts', function (event, args: { posts: any; }) {
            $scope.posts = args.posts;
        });

        $scope.showLink = function (link: any) {
            if (link != null) {
                return '<a href=" ' + link + '"><b class="icon-zoom-in"></b></a>';
            }
            else {
                return '';
            }
        }

        $scope.$on('list-append-post', function (event, args: any) {
            $scope.posts = args.concat($scope.posts);
        });

        $scope.deletePost = function (e) {
            var el = angular.element(e.srcElement);
            el.removeClass('icon-remove');
            el.children().show();
        }

        $scope.confirmDeletion = function (e, post, confirm) {
            var el = angular.element(e.srcElement);
            
            if (confirm) {
                var doc = {
                    feedId : post.FeedId,
                    postId : post.Id
                };
            
                var json = JSON.stringify(doc);

                $http.post('/manage/blog/deletepost', json).success(function (payload) {
                    if (payload.StatusCode !== 200) {
                        notification(new UserMessage(payload.ErrorDetails, MessageType.ERROR));
                    }
                    else
                        el.parent().parent().remove();
                });
            }
            else {
                el.parent().addClass('icon-remove').children().hide();
            }
        }
    }
}

class TabsController {
    constructor ($scope, $q, $rootElement, $route, $http) {
        function load(feedId: string) {
            $http.get('/manage/blog/getposts/?feedId=' + feedId).success(function (payload) {
                    $scope.$emit('data-posts',{ posts: payload.Data });
            });
        }
        
        //when page completely loads
        $rootElement.ready(function () {
            var firstTab = $('#feed_tabs li:first');
            if (firstTab == null)
                return;

            firstTab.attr('class', 'active');//select first one
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
        }

        $scope.showNewFeedDialog = function () {
            $('#feed_create').modal();
        }

        $scope.deleteFeed = function (e) {
            var el = angular.element(e.srcElement);
            el.removeClass('icon-remove');
            el.children().show();
        }

        $scope.confirmDeletion = function (e, feedId, confirm) {
            var el = angular.element(e.srcElement);
            
            if (confirm) {
                var doc = {
                    feedId : feedId
                };
            
                var json = JSON.stringify(doc);

                $http.post('/manage/blog/deletefeed', json).success(function (payload) {
                    if (payload.StatusCode != 200) {
                        //alarm(payload.StatusMessage + ":" + payload.ErrorDetails, '#message_popup');
                    }
                    else //operation successful
                        document.location.reload(true);
                });
            }
            else {
                el.parent().addClass('icon-remove').children().hide();
            }
        }
    }
}

class FeedController {
    constructor ($scope, $http) {
        $scope.newFeed = function (feed) {
            
            if (feed == undefined) {
                alert('feed must exists');
                return;
            }
            
            if (!angular.isDefined(feed.description)) {
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
                if (payload.StatusCode != 200) {
                    //alarm(payload.StatusMessage + ":" + payload.ErrorDetails, '#message_popup');
                }
                else //operation successful
                    document.location.reload(true);
            });
        }
    }
}

function countChar(val) {
    var len = val.value.length;
    if (len >= 280) {
        val.value = val.value.substring(0, 280);
    }
    else if (len >= 140) {
        $('#post_content_count').text("* " + (279 - len));//max size - 1 to make sure the remaining character count makes sense

    }
    else {
        $('#post_content_count').text((280 - len) + "");
    }
}

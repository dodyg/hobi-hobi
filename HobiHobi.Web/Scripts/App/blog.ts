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
var blogModule = angular.module('blogModule', []);
blogModule.run(function ($rootScope) {
    $rootScope.$on('data-posts', function (event, args) {
        $rootScope.$broadcast('list-posts', args);
    });

    $rootScope.$on('data-single-post', function (event, args) {
        $rootScope.$broadcast('list-append-post', args);
    });

});

var notification: any[] = ['$window', function (win) {
    return function (args: UserMessage) {
        var msg = angular.element('#user-message');
        switch (args.Type) {
            case MessageType.ERROR : msg.removeClass().addClass('alert alert-error').text(args.Message);
                break;
            case MessageType.INFO : msg.removeClass().addClass('alert alert-info').text(args.Message);
                break;
            case MessageType.SUCCESS : msg.removeClass().addClass('alert alert-success').text(args.Message);
                break;
            case MessageType.WARNING: msg.removeClass().addClass('alert').text(args.Message);
        }
    };
}];

blogModule.factory('notification', notification);

blogModule.directive('showonhoverparent',
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

class PostController {
    constructor ($scope, $q, notification) {
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
                    deferred.resolve($scope.$emit('data-single-post', [payload.Data]));
                });
            });
        }//end of $scope.newPost
    }
}

class PostListController {
    constructor ($scope, $q, notification) {
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

        $scope.showActions = function (e) {
            //notification(new UserMessage('hover', MessageType.INFO));
        };

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
            
                var deferred = $q.defer();
                var json = JSON.stringify(doc);

                $.ajax('/manage/blog/deletepost', {
                    data: json,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                }).done(function (payload) {
                    $scope.$apply(function () {
                        if (payload.StatusCode !== 200) {
                            notification(new UserMessage(payload.ErrorDetails, MessageType.ERROR));
                        }
                        else
                            deferred.resolve(el.parent().parent().remove());
                    });
                });
            }
            else {
                el.parent().addClass('icon-remove').children().hide();
            }
        }
    }
}

class TabsController {
    constructor ($scope, $q, $rootElement, $route) {
        function load(feedId: string) {

            var deferred = $q.defer();

            $.get('/manage/blog/getposts/?feedId=' + feedId, function (payload) {
                $scope.$apply(function () {
                    deferred.resolve($scope.$emit('data-posts',
                    { posts: payload.Data }));
                });
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
            
                var deferred = $q.defer();
                var json = JSON.stringify(doc);

                $.ajax('/manage/blog/deletefeed', {
                    data: json,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                }).done(function (payload) {
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
    constructor ($scope) {
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

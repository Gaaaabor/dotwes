/*! For license information please see module.js.LICENSE.txt */
define(["@grafana/data","react","@grafana/runtime","@grafana/ui","lodash"],(function(t,e,r,n,o){return function(t){var e={};function r(n){if(e[n])return e[n].exports;var o=e[n]={i:n,l:!1,exports:{}};return t[n].call(o.exports,o,o.exports,r),o.l=!0,o.exports}return r.m=t,r.c=e,r.d=function(t,e,n){r.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},r.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},r.t=function(t,e){if(1&e&&(t=r(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(r.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var o in t)r.d(n,o,function(e){return t[e]}.bind(null,o));return n},r.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return r.d(e,"a",e),e},r.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},r.p="/",r(r.s=52)}([function(e,r){e.exports=t},function(t,r){t.exports=e},function(t,e){t.exports=function(t){var e=typeof t;return null!=t&&("object"==e||"function"==e)}},function(t,e,r){var n=r(19),o=r(13),u=r(36),a=r(37),i=Object.prototype,c=i.hasOwnProperty,f=n((function(t,e){t=Object(t);var r=-1,n=e.length,f=n>2?e[2]:void 0;for(f&&u(e[0],e[1],f)&&(n=1);++r<n;)for(var s=e[r],l=a(s),p=-1,d=l.length;++p<d;){var y=l[p],v=t[y];(void 0===v||o(v,i[y])&&!c.call(t,y))&&(t[y]=s[y])}return t}));t.exports=f},function(t,e){t.exports=r},function(t,e){t.exports=n},function(t,e,r){var n=r(11),o=r(29),u=r(30),a=n?n.toStringTag:void 0;t.exports=function(t){return null==t?void 0===t?"[object Undefined]":"[object Null]":a&&a in Object(t)?o(t):u(t)}},function(t,e,r){var n=r(12),o="object"==typeof self&&self&&self.Object===Object&&self,u=n||o||Function("return this")();t.exports=u},function(t,e){t.exports=function(t){return null!=t&&"object"==typeof t}},function(t,e){t.exports=function(t){return t}},function(t,e,r){var n=r(6),o=r(2);t.exports=function(t){if(!o(t))return!1;var e=n(t);return"[object Function]"==e||"[object GeneratorFunction]"==e||"[object AsyncFunction]"==e||"[object Proxy]"==e}},function(t,e,r){var n=r(7).Symbol;t.exports=n},function(t,e,r){(function(e){var r="object"==typeof e&&e&&e.Object===Object&&e;t.exports=r}).call(this,r(28))},function(t,e){t.exports=function(t,e){return t===e||t!=t&&e!=e}},function(t,e,r){var n=r(10),o=r(15);t.exports=function(t){return null!=t&&o(t.length)&&!n(t)}},function(t,e){t.exports=function(t){return"number"==typeof t&&t>-1&&t%1==0&&t<=9007199254740991}},function(t,e){var r=/^(?:0|[1-9]\d*)$/;t.exports=function(t,e){var n=typeof t;return!!(e=null==e?9007199254740991:e)&&("number"==n||"symbol"!=n&&r.test(t))&&t>-1&&t%1==0&&t<e}},function(t,e){t.exports=function(t){return t.webpackPolyfill||(t.deprecate=function(){},t.paths=[],t.children||(t.children=[]),Object.defineProperty(t,"loaded",{enumerable:!0,get:function(){return t.l}}),Object.defineProperty(t,"id",{enumerable:!0,get:function(){return t.i}}),t.webpackPolyfill=1),t}},function(t,e){t.exports=o},function(t,e,r){var n=r(9),o=r(20),u=r(22);t.exports=function(t,e){return u(o(t,e,n),t+"")}},function(t,e,r){var n=r(21),o=Math.max;t.exports=function(t,e,r){return e=o(void 0===e?t.length-1:e,0),function(){for(var u=arguments,a=-1,i=o(u.length-e,0),c=Array(i);++a<i;)c[a]=u[e+a];a=-1;for(var f=Array(e+1);++a<e;)f[a]=u[a];return f[e]=r(c),n(t,this,f)}}},function(t,e){t.exports=function(t,e,r){switch(r.length){case 0:return t.call(e);case 1:return t.call(e,r[0]);case 2:return t.call(e,r[0],r[1]);case 3:return t.call(e,r[0],r[1],r[2])}return t.apply(e,r)}},function(t,e,r){var n=r(23),o=r(35)(n);t.exports=o},function(t,e,r){var n=r(24),o=r(25),u=r(9),a=o?function(t,e){return o(t,"toString",{configurable:!0,enumerable:!1,value:n(e),writable:!0})}:u;t.exports=a},function(t,e){t.exports=function(t){return function(){return t}}},function(t,e,r){var n=r(26),o=function(){try{var t=n(Object,"defineProperty");return t({},"",{}),t}catch(t){}}();t.exports=o},function(t,e,r){var n=r(27),o=r(34);t.exports=function(t,e){var r=o(t,e);return n(r)?r:void 0}},function(t,e,r){var n=r(10),o=r(31),u=r(2),a=r(33),i=/^\[object .+?Constructor\]$/,c=Function.prototype,f=Object.prototype,s=c.toString,l=f.hasOwnProperty,p=RegExp("^"+s.call(l).replace(/[\\^$.*+?()[\]{}|]/g,"\\$&").replace(/hasOwnProperty|(function).*?(?=\\\()| for .+?(?=\\\])/g,"$1.*?")+"$");t.exports=function(t){return!(!u(t)||o(t))&&(n(t)?p:i).test(a(t))}},function(t,e){var r;r=function(){return this}();try{r=r||new Function("return this")()}catch(t){"object"==typeof window&&(r=window)}t.exports=r},function(t,e,r){var n=r(11),o=Object.prototype,u=o.hasOwnProperty,a=o.toString,i=n?n.toStringTag:void 0;t.exports=function(t){var e=u.call(t,i),r=t[i];try{t[i]=void 0;var n=!0}catch(t){}var o=a.call(t);return n&&(e?t[i]=r:delete t[i]),o}},function(t,e){var r=Object.prototype.toString;t.exports=function(t){return r.call(t)}},function(t,e,r){var n,o=r(32),u=(n=/[^.]+$/.exec(o&&o.keys&&o.keys.IE_PROTO||""))?"Symbol(src)_1."+n:"";t.exports=function(t){return!!u&&u in t}},function(t,e,r){var n=r(7)["__core-js_shared__"];t.exports=n},function(t,e){var r=Function.prototype.toString;t.exports=function(t){if(null!=t){try{return r.call(t)}catch(t){}try{return t+""}catch(t){}}return""}},function(t,e){t.exports=function(t,e){return null==t?void 0:t[e]}},function(t,e){var r=Date.now;t.exports=function(t){var e=0,n=0;return function(){var o=r(),u=16-(o-n);if(n=o,u>0){if(++e>=800)return arguments[0]}else e=0;return t.apply(void 0,arguments)}}},function(t,e,r){var n=r(13),o=r(14),u=r(16),a=r(2);t.exports=function(t,e,r){if(!a(r))return!1;var i=typeof e;return!!("number"==i?o(r)&&u(e,r.length):"string"==i&&e in r)&&n(r[e],t)}},function(t,e,r){var n=r(38),o=r(49),u=r(14);t.exports=function(t){return u(t)?n(t,!0):o(t)}},function(t,e,r){var n=r(39),o=r(40),u=r(42),a=r(43),i=r(16),c=r(45),f=Object.prototype.hasOwnProperty;t.exports=function(t,e){var r=u(t),s=!r&&o(t),l=!r&&!s&&a(t),p=!r&&!s&&!l&&c(t),d=r||s||l||p,y=d?n(t.length,String):[],v=y.length;for(var b in t)!e&&!f.call(t,b)||d&&("length"==b||l&&("offset"==b||"parent"==b)||p&&("buffer"==b||"byteLength"==b||"byteOffset"==b)||i(b,v))||y.push(b);return y}},function(t,e){t.exports=function(t,e){for(var r=-1,n=Array(t);++r<t;)n[r]=e(r);return n}},function(t,e,r){var n=r(41),o=r(8),u=Object.prototype,a=u.hasOwnProperty,i=u.propertyIsEnumerable,c=n(function(){return arguments}())?n:function(t){return o(t)&&a.call(t,"callee")&&!i.call(t,"callee")};t.exports=c},function(t,e,r){var n=r(6),o=r(8);t.exports=function(t){return o(t)&&"[object Arguments]"==n(t)}},function(t,e){var r=Array.isArray;t.exports=r},function(t,e,r){(function(t){var n=r(7),o=r(44),u=e&&!e.nodeType&&e,a=u&&"object"==typeof t&&t&&!t.nodeType&&t,i=a&&a.exports===u?n.Buffer:void 0,c=(i?i.isBuffer:void 0)||o;t.exports=c}).call(this,r(17)(t))},function(t,e){t.exports=function(){return!1}},function(t,e,r){var n=r(46),o=r(47),u=r(48),a=u&&u.isTypedArray,i=a?o(a):n;t.exports=i},function(t,e,r){var n=r(6),o=r(15),u=r(8),a={};a["[object Float32Array]"]=a["[object Float64Array]"]=a["[object Int8Array]"]=a["[object Int16Array]"]=a["[object Int32Array]"]=a["[object Uint8Array]"]=a["[object Uint8ClampedArray]"]=a["[object Uint16Array]"]=a["[object Uint32Array]"]=!0,a["[object Arguments]"]=a["[object Array]"]=a["[object ArrayBuffer]"]=a["[object Boolean]"]=a["[object DataView]"]=a["[object Date]"]=a["[object Error]"]=a["[object Function]"]=a["[object Map]"]=a["[object Number]"]=a["[object Object]"]=a["[object RegExp]"]=a["[object Set]"]=a["[object String]"]=a["[object WeakMap]"]=!1,t.exports=function(t){return u(t)&&o(t.length)&&!!a[n(t)]}},function(t,e){t.exports=function(t){return function(e){return t(e)}}},function(t,e,r){(function(t){var n=r(12),o=e&&!e.nodeType&&e,u=o&&"object"==typeof t&&t&&!t.nodeType&&t,a=u&&u.exports===o&&n.process,i=function(){try{var t=u&&u.require&&u.require("util").types;return t||a&&a.binding&&a.binding("util")}catch(t){}}();t.exports=i}).call(this,r(17)(t))},function(t,e,r){var n=r(2),o=r(50),u=r(51),a=Object.prototype.hasOwnProperty;t.exports=function(t){if(!n(t))return u(t);var e=o(t),r=[];for(var i in t)("constructor"!=i||!e&&a.call(t,i))&&r.push(i);return r}},function(t,e){var r=Object.prototype;t.exports=function(t){var e=t&&t.constructor;return t===("function"==typeof e&&e.prototype||r)}},function(t,e){t.exports=function(t){var e=[];if(null!=t)for(var r in Object(t))e.push(r);return e}},function(t,e,r){"use strict";r.r(e);var n=r(0),o=function(t,e){return(o=Object.setPrototypeOf||{__proto__:[]}instanceof Array&&function(t,e){t.__proto__=e}||function(t,e){for(var r in e)e.hasOwnProperty(r)&&(t[r]=e[r])})(t,e)};function u(t,e){function r(){this.constructor=t}o(t,e),t.prototype=null===e?Object.create(e):(r.prototype=e.prototype,new r)}var a=function(){return(a=Object.assign||function(t){for(var e,r=1,n=arguments.length;r<n;r++)for(var o in e=arguments[r])Object.prototype.hasOwnProperty.call(e,o)&&(t[o]=e[o]);return t}).apply(this,arguments)};function i(t,e,r,n){return new(r||(r=Promise))((function(o,u){function a(t){try{c(n.next(t))}catch(t){u(t)}}function i(t){try{c(n.throw(t))}catch(t){u(t)}}function c(t){var e;t.done?o(t.value):(e=t.value,e instanceof r?e:new r((function(t){t(e)}))).then(a,i)}c((n=n.apply(t,e||[])).next())}))}function c(t,e){var r,n,o,u,a={label:0,sent:function(){if(1&o[0])throw o[1];return o[1]},trys:[],ops:[]};return u={next:i(0),throw:i(1),return:i(2)},"function"==typeof Symbol&&(u[Symbol.iterator]=function(){return this}),u;function i(u){return function(i){return function(u){if(r)throw new TypeError("Generator is already executing.");for(;a;)try{if(r=1,n&&(o=2&u[0]?n.return:u[0]?n.throw||((o=n.return)&&o.call(n),0):n.next)&&!(o=o.call(n,u[1])).done)return o;switch(n=0,o&&(u=[2&u[0],o.value]),u[0]){case 0:case 1:o=u;break;case 4:return a.label++,{value:u[1],done:!1};case 5:a.label++,n=u[1],u=[0];continue;case 7:u=a.ops.pop(),a.trys.pop();continue;default:if(!(o=a.trys,(o=o.length>0&&o[o.length-1])||6!==u[0]&&2!==u[0])){a=0;continue}if(3===u[0]&&(!o||u[1]>o[0]&&u[1]<o[3])){a.label=u[1];break}if(6===u[0]&&a.label<o[1]){a.label=o[1],o=u;break}if(o&&a.label<o[2]){a.label=o[2],a.ops.push(u);break}o[2]&&a.ops.pop(),a.trys.pop();continue}u=e.call(t,a)}catch(t){u=[6,t],n=0}finally{r=o=0}if(5&u[0])throw u[1];return{value:u[0]?u[1]:void 0,done:!0}}([u,i])}}}var f=r(3),s=r.n(f),l=r(18),p=r.n(l),d=r(4),y={},v=function(t){function e(e){var r=t.call(this,e)||this;return r.url=e.url||"",r}return u(e,t),e.prototype.query=function(t){return i(this,void 0,Promise,(function(){var e,r=this;return c(this,(function(o){return e=t.targets.map((function(e){return i(r,void 0,void 0,(function(){function r(t){var e=[];return t.forEach((function(t){var r="number"===t.type?n.FieldType.number:n.FieldType.string,o={name:t.field_name,type:r,config:{}};"color"in t&&(o.config.color={fixedColor:t.color,mode:n.FieldColorModeId.Fixed}),"displayName"in t&&(o.config.displayName=t.displayName),e.push(o)})),e}var o,u,a,i,f,l,p,v,b,h,g,x,j;return c(this,(function(c){switch(c.label){case 0:return o=s()(e,y),u=Object(d.getTemplateSrv)().replace(o.queryText,t.scopedVars),[4,this.doRequest("/api/graph/fields",""+u)];case 1:return a=c.sent(),[4,this.doRequest("/api/graph/data",""+u)];case 2:return i=c.sent(),f=a.data.nodes_fields,l=a.data.edges_fields,p={preferredVisualisationType:"nodeGraph"},v=r(f),b=new n.MutableDataFrame({name:"Nodes",refId:o.refId,fields:v,meta:p}),h=r(l),g=new n.MutableDataFrame({name:"Edges",refId:o.refId,fields:h,meta:p}),x=i.data.nodes,j=i.data.edges,x.forEach((function(t){b.add(t)})),j.forEach((function(t){g.add(t)})),[2,[b,g]]}}))}))})),[2,Promise.all(e).then((function(t){return{data:t[0]}}))]}))}))},e.prototype.doRequest=function(t,e){return i(this,void 0,void 0,(function(){return c(this,(function(r){return[2,Object(d.getBackendSrv)().datasourceRequest({method:"GET",url:this.url+"/nodegraphds"+t+((null==e?void 0:e.length)?"?"+e:"")})]}))}))},e.prototype.testDatasource=function(){return i(this,void 0,void 0,(function(){var t,e,r,n;return c(this,(function(o){switch(o.label){case 0:t="Cannot connect to API",o.label=1;case 1:return o.trys.push([1,3,,4]),[4,this.doRequest("/api/health")];case 2:return 200===(e=o.sent()).status?[2,{status:"success",message:"Success"}]:[2,{status:"error",message:e.statusText?e.statusText:t}];case 3:return r=o.sent(),p.a.isString(r)?[2,{status:"error",message:r}]:(n="",n+=r.statusText?r.statusText:t,r.data&&r.data.error&&r.data.error.code&&(n+=": "+r.data.error.code+". "+r.data.error.message),[2,{status:"error",message:n}]);case 4:return[2]}}))}))},e}(n.DataSourceApi),b=r(1),h=r.n(b),g=r(5),x=g.LegacyForms.FormField,j=function(t){function e(){var e=null!==t&&t.apply(this,arguments)||this;return e.onURLChange=function(t){var r=e.props,n=r.onOptionsChange,o=r.options,u=a(a({},o.jsonData),{url:t.target.value});n(a(a({},o),{jsonData:u}))},e}return u(e,t),e.prototype.render=function(){var t=this.props.options.jsonData;return h.a.createElement("div",{className:"gf-form-group"},h.a.createElement("div",{className:"gf-form"},h.a.createElement(x,{label:"URL",onChange:this.onURLChange,value:t.url||"",placeholder:"http://localhost:5000"})))},e}(b.PureComponent),m=g.LegacyForms.FormField,O=function(t){function e(){var e=null!==t&&t.apply(this,arguments)||this;return e.onQueryTextChange=function(t){var r=e.props,n=r.onChange,o=r.query;n(a(a({},o),{queryText:t.target.value}))},e}return u(e,t),e.prototype.render=function(){var t=s()(this.props.query,y).queryText;return h.a.createElement("div",{className:"gf-form"},h.a.createElement(m,{labelWidth:8,inputWidth:20,value:t||"",onChange:this.onQueryTextChange,label:"Query String",tooltip:"The query string for data endpoint of the node graph api; i.e. /api/graph/data?query=sometext"}))},e}(b.PureComponent);r.d(e,"plugin",(function(){return w}));var w=new n.DataSourcePlugin(v).setConfigEditor(j).setQueryEditor(O)}])}));
//# sourceMappingURL=module.js.map
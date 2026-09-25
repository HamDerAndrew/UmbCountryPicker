import { css as b, property as h, state as y, query as E, customElement as $, repeat as N, html as _ } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement as U } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent as x } from "@umbraco-cms/backoffice/event";
import { UMB_CURRENT_USER_CONTEXT as P } from "@umbraco-cms/backoffice/current-user";
import { umbHttpClient as S } from "@umbraco-cms/backoffice/http-client";
import { tryExecute as T } from "@umbraco-cms/backoffice/resources";
async function O(t, e) {
  const { data: r, error: o } = await T(
    t,
    S.get({
      url: "/umbraco/management/api/v1/country-picker/countries",
      query: { languageIsoCodeString: e },
      security: [{ scheme: "bearer", type: "http" }]
    })
  );
  if (o)
    throw o;
  return r ?? [];
}
var L = Object.defineProperty, k = Object.getOwnPropertyDescriptor, v = (t) => {
  throw TypeError(t);
}, s = (t, e, r, o) => {
  for (var a = o > 1 ? void 0 : o ? k(e, r) : e, c = t.length - 1, l; c >= 0; c--)
    (l = t[c]) && (a = (o ? l(e, r, a) : l(a)) || a);
  return o && a && L(e, r, a), a;
}, m = (t, e, r) => e.has(t) || v("Cannot " + r), d = (t, e, r) => (m(t, e, "read from private field"), r ? r.call(t) : e.get(t)), f = (t, e, r) => e.has(t) ? v("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, r), q = (t, e, r, o) => (m(t, e, "write to private field"), e.set(t, r), r), p = (t, e, r) => (m(t, e, "access private method"), r), u, n, C, w, g;
let i = class extends U {
  constructor() {
    super(), f(this, n), this.readonly = !1, this._countries = [], this._filtered = [], f(this, u, "en"), this.consumeContext(P, (t) => {
      this.observe(t == null ? void 0 : t.languageIsoCode, (e) => {
        q(this, u, e ?? "en"), p(this, n, C).call(this);
      });
    });
  }
  async focus() {
    var t;
    await this.updateComplete, (t = this._input) == null || t.focus();
  }
  render() {
    return _`
			<uui-combobox
				id="input"
				label=${this.localize.term("general_country")}
				.value=${this.value ?? ""}
				?readonly=${this.readonly}
				?required=${this.mandatory}
				@search=${p(this, n, w)}
				@change=${p(this, n, g)}>
				<uui-combobox-list>
					${N(
      this._filtered,
      (t) => t.id,
      (t) => _`
							<uui-combobox-list-option .value=${t.id} .displayValue=${t.countryName}>
								${t.countryName}
							</uui-combobox-list-option>
						`
    )}
				</uui-combobox-list>
			</uui-combobox>
		`;
  }
};
u = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakSet();
C = async function() {
  const t = await O(this, d(this, u));
  this._countries = [...t].sort((e, r) => e.countryName.localeCompare(r.countryName, d(this, u))), this._filtered = this._countries;
};
w = function(t) {
  const e = t.currentTarget.search;
  if (!e) {
    this._filtered = this._countries;
    return;
  }
  const r = e.toLowerCase();
  this._filtered = this._countries.filter(
    (o) => o.countryName.toLowerCase().includes(r) || o.id.toLowerCase() === r
  );
};
g = function(t) {
  const e = t.currentTarget.value, r = typeof e == "string" && e.length ? e : void 0;
  r !== this.value && (this.value = r, this.dispatchEvent(new x()));
};
i.styles = [
  b`
			#input {
				width: 100%;
			}
		`
];
s([
  h({ type: String })
], i.prototype, "value", 2);
s([
  h({ type: Boolean, reflect: !0 })
], i.prototype, "readonly", 2);
s([
  h({ type: Boolean })
], i.prototype, "mandatory", 2);
s([
  y()
], i.prototype, "_countries", 2);
s([
  y()
], i.prototype, "_filtered", 2);
s([
  E("#input")
], i.prototype, "_input", 2);
i = s([
  $("umb-country-picker")
], i);
export {
  i as default
};
//# sourceMappingURL=country-picker.js.map

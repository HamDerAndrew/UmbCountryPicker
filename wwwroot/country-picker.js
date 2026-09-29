import { css as e, customElement as t, html as n, property as r, query as i, repeat as a, state as o } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement as s } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent as c } from "@umbraco-cms/backoffice/event";
import { UMB_CURRENT_USER_CONTEXT as l } from "@umbraco-cms/backoffice/current-user";
import { umbHttpClient as u } from "@umbraco-cms/backoffice/http-client";
import { tryExecute as d } from "@umbraco-cms/backoffice/resources";
//#region src/api.ts
async function f(e, t) {
	let { data: n, error: r } = await d(e, u.get({
		url: "/umbraco/management/api/v1/country-picker/countries",
		query: { languageIsoCodeString: t },
		security: [{
			scheme: "bearer",
			type: "http"
		}]
	}));
	if (r) throw r;
	return n ?? [];
}
//#endregion
//#region \0@oxc-project+runtime@0.151.0/helpers/esm/decorate.js
function p(e, t, n, r) {
	var i = arguments.length, a = i < 3 ? t : r === null ? r = Object.getOwnPropertyDescriptor(t, n) : r, o;
	if (typeof Reflect == "object" && typeof Reflect.decorate == "function") a = Reflect.decorate(e, t, n, r);
	else for (var s = e.length - 1; s >= 0; s--) (o = e[s]) && (a = (i < 3 ? o(a) : i > 3 ? o(t, n, a) : o(t, n)) || a);
	return i > 3 && a && Object.defineProperty(t, n, a), a;
}
//#endregion
//#region src/country-picker.element.ts
var m = class extends s {
	#e;
	constructor() {
		super(), this.readonly = !1, this._countries = [], this._filtered = [], this.#e = "en", this.consumeContext(l, (e) => {
			this.observe(e?.languageIsoCode, (e) => {
				this.#e = e ?? "en", this.#t();
			});
		});
	}
	async focus() {
		await this.updateComplete, this._input?.focus();
	}
	async #t() {
		let e = await f(this, this.#e);
		this._countries = [...e].sort((e, t) => e.countryName.localeCompare(t.countryName, this.#e)), this._filtered = this._countries;
	}
	#n(e) {
		let t = e.currentTarget.search;
		if (!t) {
			this._filtered = this._countries;
			return;
		}
		let n = t.toLowerCase();
		this._filtered = this._countries.filter((e) => e.countryName.toLowerCase().includes(n) || e.id.toLowerCase() === n);
	}
	#r() {
		this._input && this.value && !this._input.search && (this._input.value = this.value);
	}
	#i(e) {
		let t = e.currentTarget.value, n = typeof t == "string" && t.length ? t : void 0;
		n !== this.value && (this.value = n, this.dispatchEvent(new c()));
	}
	render() {
		return n`
			<uui-combobox
				id="input"
				label=${this.localize.term("general_country")}
				.value=${this.value ?? ""}
				?readonly=${this.readonly}
				?required=${this.mandatory}
				@search=${this.#n}
				@change=${this.#i}>
				<uui-combobox-list @inner-slot-change=${this.#r}>
					${a(this._filtered, (e) => e.id, (e) => n`
							<uui-combobox-list-option .value=${e.id} .displayValue=${e.countryName}>
								${e.countryName}
							</uui-combobox-list-option>
						`)}
				</uui-combobox-list>
			</uui-combobox>
		`;
	}
	static {
		this.styles = [e`
			#input {
				width: 100%;
			}
		`];
	}
};
p([r({ type: String })], m.prototype, "value", void 0), p([r({
	type: Boolean,
	reflect: !0
})], m.prototype, "readonly", void 0), p([r({ type: Boolean })], m.prototype, "mandatory", void 0), p([o()], m.prototype, "_countries", void 0), p([o()], m.prototype, "_filtered", void 0), p([i("#input")], m.prototype, "_input", void 0), m = p([t("umb-country-picker")], m);
var h = m;
//#endregion
export { h as default };

//# sourceMappingURL=country-picker.js.map
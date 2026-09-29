import { css, customElement, html, property, query, repeat, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UMB_CURRENT_USER_CONTEXT } from '@umbraco-cms/backoffice/current-user';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';
import type { UUIComboboxElement } from '@umbraco-cms/backoffice/external/uui';
import { getCountries, type CountryOption } from './api.js';

/**
 * Property editor UI for UmbCountryPicker. The stored value is the country's ISO code.
 * @element umb-country-picker
 */
@customElement('umb-country-picker')
export default class UmbCountryPickerElement extends UmbLitElement implements UmbPropertyEditorUiElement {
	@property({ type: String })
	value?: string;

	@property({ type: Boolean, reflect: true })
	readonly = false;

	@property({ type: Boolean })
	mandatory?: boolean;

	@state()
	private _countries: Array<CountryOption> = [];

	@state()
	private _filtered: Array<CountryOption> = [];

	@query('#input')
	private _input?: UUIComboboxElement;

	#languageIsoCode = 'en';

	constructor() {
		super();

		this.consumeContext(UMB_CURRENT_USER_CONTEXT, (context) => {
			this.observe(context?.languageIsoCode, (languageIsoCode) => {
				this.#languageIsoCode = languageIsoCode ?? 'en';
				this.#loadCountries();
			});
		});
	}

	override async focus() {
		await this.updateComplete;
		this._input?.focus();
	}

	async #loadCountries() {
		const countries = await getCountries(this, this.#languageIsoCode);

		// The API already orders by name, but localeCompare orders correctly for the editor's language.
		this._countries = [...countries].sort((a, b) => a.countryName.localeCompare(b.countryName, this.#languageIsoCode));
		this._filtered = this._countries;
	}

	#onSearch(event: Event) {
		const search = (event.currentTarget as UUIComboboxElement).search;
		if (!search) {
			this._filtered = this._countries;
			return;
		}

		const term = search.toLowerCase();
		this._filtered = this._countries.filter(
			(country) => country.countryName.toLowerCase().includes(term) || country.id.toLowerCase() === term,
		);
	}

	#onOptionsChange() {
		// uui-combobox only resolves its display text when its value is set. The saved value arrives before the
		// countries are fetched, so re-apply it once the options exist, otherwise the input stays blank on load.
		// Skipped while searching so the text the editor is typing is left alone.
		if (!this._input || !this.value || this._input.search) return;
		this._input.value = this.value;
	}

	#onChange(event: Event) {
		// uui-combobox types its value as a form value; this editor only ever stores an ISO code string.
		const rawValue = (event.currentTarget as UUIComboboxElement).value;
		const newValue = typeof rawValue === 'string' && rawValue.length ? rawValue : undefined;
		if (newValue === this.value) return;

		this.value = newValue;
		this.dispatchEvent(new UmbChangeEvent());
	}

	override render() {
		return html`
			<uui-combobox
				id="input"
				label=${this.localize.term('general_country')}
				.value=${this.value ?? ''}
				?readonly=${this.readonly}
				?required=${this.mandatory}
				@search=${this.#onSearch}
				@change=${this.#onChange}>
				<uui-combobox-list @inner-slot-change=${this.#onOptionsChange}>
					${repeat(
						this._filtered,
						(country) => country.id,
						(country) => html`
							<uui-combobox-list-option .value=${country.id} .displayValue=${country.countryName}>
								${country.countryName}
							</uui-combobox-list-option>
						`,
					)}
				</uui-combobox-list>
			</uui-combobox>
		`;
	}

	static override styles = [
		css`
			#input {
				width: 100%;
			}
		`,
	];
}

declare global {
	interface HTMLElementTagNameMap {
		'umb-country-picker': UmbCountryPickerElement;
	}
}

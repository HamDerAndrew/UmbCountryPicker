import { umbHttpClient } from '@umbraco-cms/backoffice/http-client';
import { tryExecute } from '@umbraco-cms/backoffice/resources';
import type { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';

export interface CountryOption {
	id: string;
	countryName: string;
}

/**
 * Fetches the country list from the package's own Management API controller.
 * @param host the controller host used to scope the request
 * @param languageIsoCodeString the editor's culture, used to localize the country names
 */
export async function getCountries(
	host: UmbControllerHost,
	languageIsoCodeString: string,
): Promise<Array<CountryOption>> {
	const { data, error } = await tryExecute(
		host,
		umbHttpClient.get<Array<CountryOption>>({
			url: '/umbraco/management/api/v1/country-picker/countries',
			query: { languageIsoCodeString },
			security: [{ scheme: 'bearer', type: 'http' }],
		}),
	);

	if (error) {
		throw error;
	}

	return data ?? [];
}

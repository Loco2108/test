/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export enum HatProfileEnum {
  Hat01 = "Hat01",
  Hat02 = "Hat02",
  Hat03 = "Hat03",
  Hat04 = "Hat04",
  Hat05 = "Hat05",
}

export enum FaceProfileEnum {
  Face01 = "Face01",
  Face02 = "Face02",
  Face03 = "Face03",
  Face04 = "Face04",
  Face05 = "Face05",
}

export enum ColorProfileEnum {
  Green = "Green",
  Blue = "Blue",
  Red = "Red",
  Purple = "Purple",
  Yellow = "Yellow",
}

export enum BodyProfileEnum {
  Body01 = "Body01",
  Body02 = "Body02",
  Body03 = "Body03",
  Body04 = "Body04",
  Body05 = "Body05",
}

export interface UserAuthDto {
  /** @format uuid */
  id?: string;
  username?: string | null;
  email?: string | null;
  profilePictureUrl?: string | null;
}

export interface UserLoginDto {
  password?: string | null;
  email?: string | null;
  staySignedIn?: boolean;
}

export interface UserRegisterDto {
  username?: string | null;
  password?: string | null;
  email?: string | null;
}

import type {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  HeadersDefaults,
  ResponseType,
} from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams
  extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown>
  extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  JsonApi = "application/vnd.api+json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({
    securityWorker,
    secure,
    format,
    ...axiosConfig
  }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({
      ...axiosConfig,
      baseURL: axiosConfig.baseURL || "",
    });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(
    params1: AxiosRequestConfig,
    params2?: AxiosRequestConfig,
  ): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method &&
          this.instance.defaults.headers[
            method.toLowerCase() as keyof HeadersDefaults
          ]) ||
          {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] =
        property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(
          key,
          isFileType ? formItem : this.stringifyFormItem(formItem),
        );
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (
      type === ContentType.FormData &&
      body &&
      body !== null &&
      typeof body === "object"
    ) {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (
      type === ContentType.Text &&
      body &&
      body !== null &&
      typeof body !== "string"
    ) {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title Backend
 * @version 1.0
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags User
     * @name V1UserRegisterCreate
     * @request POST:/api/v1/User/register
     */
    v1UserRegisterCreate: (data: UserRegisterDto, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/v1/User/register`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name V1UserLoginCreate
     * @request POST:/api/v1/User/login
     */
    v1UserLoginCreate: (data: UserLoginDto, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/v1/User/login`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name V1UserLogoutCreate
     * @request POST:/api/v1/User/logout
     */
    v1UserLogoutCreate: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/v1/User/logout`,
        method: "POST",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name V1UserCheckUsernameCreate
     * @request POST:/api/v1/User/checkUsername
     */
    v1UserCheckUsernameCreate: (
      query?: {
        username?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/v1/User/checkUsername`,
        method: "POST",
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name V1UserCheckEmailCreate
     * @request POST:/api/v1/User/checkEmail
     */
    v1UserCheckEmailCreate: (
      query?: {
        email?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/v1/User/checkEmail`,
        method: "POST",
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name V1UserMeList
     * @request GET:/api/v1/User/me
     */
    v1UserMeList: (params: RequestParams = {}) =>
      this.request<UserAuthDto, any>({
        path: `/api/v1/User/me`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
}

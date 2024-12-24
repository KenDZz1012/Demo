import axios, { AxiosResponse, AxiosRequestConfig } from "axios";
import { message, notification } from "antd";
import { Warning, Error } from "./notification";

// Define the types for the parameters
type HttpMethod = "GET" | "POST" | "PUT" | "DELETE" | "PATCH";
type ContentType = "application/json" | "multipart/form-data" | string;

interface ConnectionParams {
  URI: string;
  method?: HttpMethod;
  body?: any;
  params?: any;
  Type?: ContentType;
}

interface ConnectionApiThirdPartyParams {
  URL: string;
  method?: HttpMethod;
  body?: any;
  params?: any;
  Type?: ContentType;
  headers?: Record<string, string>;
  responseType?: AxiosRequestConfig["responseType"];
}

interface ExposeDataParams {
  ObjectData: AxiosResponse<any>;
  ShowToast: boolean; // Show the toast notification
}

interface MessengerErrorParams {
  ObjectTrycatch: any;
}

const Connection = async ({ URI, method = "GET", body, params = null, Type = "application/json" }: ConnectionParams): Promise<AxiosResponse<any>> => {
  let UrlBase: string | undefined;

  if (process.env.NODE_ENV === "development") {
    UrlBase = `${process.env.REACT_APP_PUBLIC_URL_DEV}${URI}`;
  } else if (process.env.NODE_ENV === "production") {
    UrlBase = `/api-gw/${URI}`;
  } else if (process.env.NODE_ENV === "test") {
    UrlBase = `${process.env.REACT_APP_PUBLIC_URL_TEST}${URI}`;
  }

  try {
    return await axios(UrlBase || "", {
      method,
      headers: {
        "Content-Type": Type,
        Authorization: `Bearer ${localStorage.getItem("Token")}`,
      },
      params: {
        ...params,
      },
      data: body,
    });
  } catch (error) {
    MessengerError({ ObjectTrycatch: error });
    throw error;
  }
};

export const connectionApiThirdParty = async ({
  URL,
  method = "GET",
  body,
  params = null,
  Type = "application/json",
  headers = {},
  responseType,
}: ConnectionApiThirdPartyParams): Promise<AxiosResponse<any>> => {
  try {
    return await axios(URL, {
      method,
      headers,
      params,
      data: body,
      responseType,
    });
  } catch (error) {
    MessengerError({ ObjectTrycatch: error });
    throw error;
  }
};

const ShowMessenger = (typeMessenger: string, Title: string): void => {
  message.destroy();
  switch (typeMessenger) {
    case "Info":
      message.info(Title);
      break;
    case "warning":
      message.warning(Title);
      break;
    case "error":
      message.error(Title);
      break;
    default:
      break;
  }
};

export const HttpRequest = async (method: HttpMethod = "GET", URI: string, body?: any, messageShow: boolean = false, params?: Record<string, any>, Type?: ContentType): Promise<any> => {
  if (messageShow) {
    const hide = message.loading("Đang tải dữ liệu", 0);
    setTimeout(hide, parseInt(process.env.REACT_APP_TIMEOUT || "3000", 10));
  }

  try {
    const data = await Connection({ URI, method, body, params, Type });
    return ExposeData({ ObjectData: data, ShowToast: messageShow });
  } catch (error) {
    MessengerError({ ObjectTrycatch: error });
    throw error;
  }
};

export const HttpRequestFile = async (method: HttpMethod = "GET", URI: string, body?: any, messageShow: boolean = false, params?: Record<string, any>, Type?: ContentType): Promise<any> => {
  if (messageShow) {
    const hide = message.loading("Đang tải dữ liệu", 0);
    setTimeout(hide, parseInt(process.env.REACT_APP_TIMEOUT || "3000", 10));
  }

  try {
    return await Connection({ URI, method, body, params, Type });
  } catch (error) {
    MessengerError({ ObjectTrycatch: error });
    throw error;
  }
};

const ExposeData = ({ ObjectData, ShowToast }: ExposeDataParams): any => {
  const { status, data } = ObjectData;

  if (ShowToast) {
    switch (status) {
      case 200:
        ShowMessenger("Info", "Success!");
        break;
      case 201:
        ShowMessenger("Info", "Create new success!");
        break;
      case 204:
        console.log("No content");
        break;
      default:
        ShowMessenger("error", "Error backend!");
        break;
    }
  }
  return data ?? [];
};

const MessengerError = ({ ObjectTrycatch }: MessengerErrorParams): any[] => {
  const { code, message: errorMessage, request } = ObjectTrycatch;

  if (code === "ERR_BAD_REQUEST") {
    const {
      response: { data: messenger },
    } = ObjectTrycatch;

    if (request.status === 401) {
      window.location.href = "/login";
    }
    Warning({ description: messenger.messenger, message: "Thông báo" });
  } else if (code === "ERR_NETWORK" || code === "ERR_BAD_RESPONSE") {
    Error({ description: errorMessage, message: "Thông báo" });
  }

  ShowMessenger("error", "Error backend!");
  return [];
};

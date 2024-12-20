import { notification } from "antd";

export const Success = (config: any) => {
  notification.success(config);
};
export const Error = (config: any) => {
  notification.error(config);
};

export const Warning = (config: any) => {
  notification.warning(config);
};
export const Info = (config: any) => {
  notification.info(config);
};

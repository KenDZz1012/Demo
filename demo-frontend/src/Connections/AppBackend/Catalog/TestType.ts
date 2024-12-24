import { ITestCode } from "../../../Interface/ITestCode";
import { HttpRequest } from "../../Connection";

export const FetchTestCodes = async (): Promise<ITestCode[]> => {
  return await HttpRequest("GET", `ca/testtype`);
};

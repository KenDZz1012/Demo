import { Table } from "antd";

interface CustomTableProps {
  columns: Array<any>; // Define the type of your `column` object if possible
  data: any; // Define the type of your `data` array or object
  loading: boolean;
  style: any;
}

const CustomTable: React.FC<CustomTableProps> = ({ columns, data, loading, style }) => {
  return <Table style={{ ...style, width: "100%" }} dataSource={data} loading={loading} columns={columns} />;
};

export default CustomTable;

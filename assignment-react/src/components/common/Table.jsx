import React from "react";

const Table = ({ data }) => {
  if (!data || data.length === 0) {
    return <div>No data available</div>;
  }

  const headers = Object.keys(data[0]);

  return (
    <table className="min-w-full divide-y divide-gray-200">
      <thead className="bg-gray-50">
        <tr>
          {headers.map((header) => (
            <th
              key={header}
              className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
            >
              {header}
            </th>
          ))}
          <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
            Action
          </th>
        </tr>
      </thead>
      <tbody className="bg-white divide-y divide-gray-200">
        {data.map((item, index) => (
          <tr key={index}>
            {headers.map((header) => (
              <td key={header} className="px-6 py-4 whitespace-nowrap">
                {item[header]}
              </td>
            ))}
            <td className="px-6 py-4 whitespace-nowrap">{item.action}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Table;
